using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;

namespace Piano.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OefeningController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OefeningController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> SlaScoreOp(int goedeAntwoorden, int aantalVragen, int oefeningId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Ok(new
                {
                    succes = true,
                    message = "Login om je score op te slaan!",
                    isGast = true
                });
            }

            int score = (int)Math.Round((double)goedeAntwoorden / aantalVragen * 100);

            var resultaat = new Resultaat
            {
                Score = score,
                GoedeAntwoorden = goedeAntwoorden,
                AantalVragen = aantalVragen,
                OefeningId = oefeningId,
                UserId = userId,
                datetime = DateTime.Now
            };

            _context.Resultaten.Add(resultaat);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                succes = true,
                message = "Score succesvol opgeslagen",
                resultaat,
                isGast = false
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<Oefening>>> GetAll()
        {
            // return await _context.Oefeningen.ToListAsync();
            var oefeningen = await _context.Oefeningen.ToListAsync();
            return Ok(oefeningen);
        }
    }
}