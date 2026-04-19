using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;

namespace Piano.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LeerlingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeerlingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Leerling>>> GetAll()
        {
            var leerlingen = await _context.Leerlingen.ToListAsync();
            return Ok(leerlingen);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Leerling>> GetById(int id)
        {
            var leerling = await _context.Leerlingen.FindAsync(id);
            if (leerling == null) return NotFound();
            return Ok(leerling);
        }
    }
}