using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;

namespace Piano.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class KlasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KlasController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klas>>> GetMijnKlassen()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);

            if (docent == null) return NotFound("Docent niet gevonden");

            var klassen = await _context.Klassen
                .Include(k => k.Docent)
                .Include(k => k.Leerlingen)
                .Where(k => k.DocentIdentityUserId == docent.IdentityUserId)
                .ToListAsync();

            return Ok(klassen);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<KlasDetailsDto>> GetById(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);

            if (docent == null) return NotFound("Docent niet gevonden");

            var klas = await _context.Klassen
                .Include(k => k.Docent)
                .Include(k => k.Leerlingen)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (klas == null) return NotFound();

            if (klas.DocentIdentityUserId != docent.IdentityUserId)
                return Forbid();

            var result = new KlasDetailsDto
            {
                Id = klas.Id,
                Naam = klas.Naam,
                DocentNaam = klas.Docent?.Naam ?? "Onbekend",
                Leerlingen = klas.Leerlingen.Select(l => new LeerlingDto
                {
                    Id = l.Id,
                    Naam = l.Naam,
                    Niveau = l.Niveau.ToString() ?? "Beginner"
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Klas>> CreateKlas([FromBody] CreateKlasDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);

            if (docent == null) return NotFound();

            var klas = new Klas
            {
                Naam = dto.Naam,
                DocentIdentityUserId = docent.IdentityUserId
            };

            _context.Klassen.Add(klas);
            await _context.SaveChangesAsync();

            return Ok(klas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKlas(int id, [FromBody] UpdateKlasDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var klas = await _context.Klassen.FindAsync(id);
            if (klas == null)
                return NotFound();

            // Alleen de eigenaar docent mag de klas wijzigen
            if (klas.DocentIdentityUserId != currentUser.Id)
                return Forbid();

            klas.Naam = dto.Naam;
            await _context.SaveChangesAsync();

            return Ok(klas);
        }

        [HttpPost("{id}/leerling")]
        public async Task<IActionResult> AddLeerlingToKlas(int id, [FromBody] AddLeerlingDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);

            if (docent == null)
                return NotFound("Docent niet gevonden");

            var klas = await _context.Klassen.FindAsync(id);
            if (klas == null)
                return NotFound();

            if (klas.DocentIdentityUserId != currentUser.Id)
                return Forbid();

            var leerlingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (leerlingUser == null)
                return NotFound("Leerling met dit e-mailadres niet gevonden");

            var isDocent = await _userManager.IsInRoleAsync(leerlingUser, "Docent");
            if (isDocent)
                return BadRequest("De opgegeven gebruiker is een docent, geen leerling");

            var isLeerling = await _userManager.IsInRoleAsync(leerlingUser, "Leerling");
            if (!isLeerling)
                return BadRequest("De opgegeven gebruiker heeft geen leerling rol");

            var bestaandeLeerling = await _context.Leerlingen.FirstOrDefaultAsync(l => l.IdentityUserId == leerlingUser.Id);

            if (bestaandeLeerling != null)
            {
                if (bestaandeLeerling.KlasId == id)
                {
                    return BadRequest("Deze leerling zit al in deze klas");
                }

                if (bestaandeLeerling.KlasId != null && bestaandeLeerling.KlasId != id)
                {
                    return BadRequest("Deze leerling zit al in een andere klas");
                }
            }

            Leerling leerling;
            if (bestaandeLeerling != null)
            {
                bestaandeLeerling.KlasId = id;
                leerling = bestaandeLeerling;

                if (bestaandeLeerling.Niveau == null)
                    bestaandeLeerling.Niveau = dto.Niveau ?? Niveau.Beginner;
            }
            else
            {
                leerling = new Leerling
                {
                    Naam = dto.Naam ?? leerlingUser.UserName ?? dto.Email,
                    Niveau = dto.Niveau ?? Niveau.Beginner,
                    IdentityUserId = leerlingUser.Id,
                    KlasId = id
                };
                _context.Leerlingen.Add(leerling);
            }


            await _context.SaveChangesAsync();

            return Ok(leerling);
        }

        [HttpDelete("{klasId}/leerling/{leerlingId}")]
        public async Task<IActionResult> DeleteLeerlingFromKlas(int klasId, int leerlingId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var docent = await _context.Docenten.FirstOrDefaultAsync(d => d.IdentityUserId == currentUser.Id);
            if (docent == null) return NotFound("Docent niet gevonden");

            var klas = await _context.Klassen.FindAsync(klasId);
            if (klas == null)
                return NotFound();

            if (klas.DocentIdentityUserId != currentUser.Id)
                return Forbid();

            var leerling = await _context.Leerlingen.FindAsync(leerlingId);
            if (leerling == null || leerling.KlasId != klasId)
                return NotFound();

            leerling.KlasId = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Leerling uit klas verwijderd" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlas(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var klas = await _context.Klassen.FindAsync(id);
            if (klas == null)
                return NotFound();

            if (klas.DocentIdentityUserId != currentUser.Id)
                return Forbid();

            _context.Klassen.Remove(klas);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Klas verwijderd" });
        }
    }

    public class CreateKlasDto
    {
        public string Naam { get; set; } = string.Empty;
    }

    public class UpdateKlasDto
    {
        public string Naam { get; set; } = string.Empty;
    }

    public class AddLeerlingDto
    {
        public string Naam { get; set; } = string.Empty;
        public Niveau? Niveau { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class KlasDetailsDto
    {
        public int Id { get; set; }
        public string Naam { get; set; } = string.Empty;
        public string DocentNaam { get; set; } = string.Empty;
        public List<LeerlingDto> Leerlingen { get; set; } = new List<LeerlingDto>();
    }

    public class LeerlingDto
    {
        public int Id { get; set; }
        public string Naam { get; set; } = string.Empty;
        public string Niveau { get; set; } = string.Empty;
    }
}