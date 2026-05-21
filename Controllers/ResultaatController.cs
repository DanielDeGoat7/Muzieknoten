using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;
using System.Security.Claims;

namespace Piano.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ResultaatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ResultaatController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("mijnresultaten")]
        public async Task<ActionResult<List<ResultaatDto>>> GetMijnResultaten()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("Gebruiker niet gevonden");
            }

            var resultaten = await _context.Resultaten
                .Where(r => r.UserId == currentUser.Id)
                .Include(r => r.Oefening)
                .OrderByDescending(r => r.datetime)
                .Select(r => new ResultaatDto
                {
                    Id = r.Id,
                    Score = r.Score,
                    GoedeAntwoorden = r.GoedeAntwoorden,
                    AantalVragen = r.AantalVragen,
                    OefeningNaam = r.Oefening != null ? r.Oefening.Naam : "Onbekend",
                    Datum = r.datetime,
                    Percentage = r.AantalVragen > 0 ? (int)Math.Round((double)r.GoedeAntwoorden / r.AantalVragen * 100) : 0
                })
                .ToListAsync();

            return Ok(resultaten);
        }

        [HttpGet("mijnleerlingen")]
        public async Task<ActionResult<List<LeerlingOverzichtDto>>> GetMijnLeerlingen()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("Gebruiker niet gevonden");
            }

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);

            if (docent == null)
                return NotFound("Docent niet gevonden");

            var klassen = await _context.Klassen
                .Include(k => k.Leerlingen)
                .Where(k => k.DocentIdentityUserId == docent.IdentityUserId)
                .ToListAsync();

            var leerlingenOverzicht = new List<LeerlingOverzichtDto>();

            foreach (var klas in klassen)
            {
                foreach (var leerling in klas.Leerlingen)
                {
                    var leerlingUser = await _userManager.FindByIdAsync(leerling.IdentityUserId);

                    var resultaten = await _context.Resultaten
                        .Where(r => r.UserId == leerling.IdentityUserId)
                        .ToListAsync();

                    leerlingenOverzicht.Add(new LeerlingOverzichtDto
                    {
                        LeerlingId = leerling.Id,
                        Email = leerlingUser?.Email ?? "Onbekend",
                        Niveau = leerling.Niveau?.ToString() ?? "Beginner",
                        KlasNaam = klas.Naam,
                        AantalResultaten = resultaten.Count
                    });

                }
            }

            return Ok(leerlingenOverzicht.OrderBy(l => l.KlasNaam).ThenBy(l => l.Email));
        }

        [HttpGet("leerlingvoortgang/{leerlingId}")]
        public async Task<ActionResult<LeerlingVoortgangDto>> GetLeerlingVoortgang(int leerlingId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("Gebruiker niet gevonden");
            }

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);
            if (docent == null)
                return NotFound("Docent niet gevonden");

            var leerling = await _context.Leerlingen
                .Include(l => l.Klas)
                .FirstOrDefaultAsync(l => l.Id == leerlingId);

            if (leerling == null)
                return NotFound("Leerling niet gevonden");

            if (leerling.Klas == null || leerling.Klas.DocentIdentityUserId != docent.IdentityUserId)
                return Forbid("Je hebt geen toegang tot deze leerling");

            var leerlingUser = await _userManager.FindByIdAsync(leerling.IdentityUserId);

            var resultaten = await _context.Resultaten
                .Where(r => r.UserId == leerling.IdentityUserId)
                .Include(r => r.Oefening)
                .OrderByDescending(r => r.datetime)
                .Select(r => new ResultaatDto
                {
                    Id = r.Id,
                    Score = r.Score,
                    GoedeAntwoorden = r.GoedeAntwoorden,
                    AantalVragen = r.AantalVragen,
                    OefeningNaam = r.Oefening != null ? r.Oefening.Naam : "Onbekend",
                    Datum = r.datetime,
                    Percentage = (double)r.GoedeAntwoorden / r.AantalVragen * 100
                })
                .ToListAsync();

            var result = new LeerlingVoortgangDto
            {
                LeerlingEmail = leerlingUser?.Email ?? "Onbekend",
                KlasNaam = leerling.Klas?.Naam ?? "Onbekend",
                Resultaten = resultaten
            };

            return Ok(result);
        }
    }

    public class ResultaatDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int GoedeAntwoorden { get; set; }
        public int AantalVragen { get; set; }
        public string OefeningNaam { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public double Percentage { get; set; }
        public string WeergaveScore => $"{GoedeAntwoorden}/{AantalVragen} ({Percentage:F0}%)";
    }

    public class LeerlingOverzichtDto
    {
        public int LeerlingId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Niveau { get; set; } = string.Empty;
        public string KlasNaam { get; set; } = string.Empty;
        public int AantalResultaten { get; set; }
    }

    public class LeerlingVoortgangDto
    {
        public string LeerlingEmail { get; set; } = string.Empty;
        public string KlasNaam { get; set; } = string.Empty;
        public List<ResultaatDto> Resultaten { get; set; } = new List<ResultaatDto>();
    }
}