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

        [HttpGet]
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

}