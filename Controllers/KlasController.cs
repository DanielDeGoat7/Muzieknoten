using Microsoft.AspNetCore.Http;
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

        public KlasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Klas>>> GetAll()
        {
            var klassen = await _context.Klassen
                .Include(k => k.Docent)
                .Include(k => k.Leerlingen)
                .ToListAsync();
            return Ok(klassen);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Klas>> GetById(int id)
        {
            var klas = await _context.Klassen.FindAsync(id);
            if (klas == null) return NotFound();
            return Ok(klas);
        }
    }
}