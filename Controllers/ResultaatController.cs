using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;

namespace Piano.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ResultaatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ResultaatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Resultaat>>> GetAll()
        {
            // return await _context.Resultaten.ToListAsync();
            var resultaten = await _context.Resultaten.ToListAsync();
            return Ok(resultaten);
        }

        [HttpPost]
        public async Task<ActionResult<Resultaat>> Create(Resultaat nieuwResultaat)
        {
            // Mitigatie: ID6 - Validatie van invoer
            if (nieuwResultaat.Score < 0 || nieuwResultaat.Score > 100)
            {
                return BadRequest("Ongeldige score");
            }
            _context.Resultaten.Add(nieuwResultaat);
            await _context.SaveChangesAsync();

            return Ok(nieuwResultaat);
        }

    }
}