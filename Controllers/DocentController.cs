using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piano.Data;
using Piano.Models;

namespace Piano.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DocentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DocentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Docent>>> GetAll()
        {
            var docenten = await _context.Docenten.ToListAsync();
            return Ok(docenten);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Docent>> GetById(int id)
        {
            var docent = await _context.Docenten.FindAsync(id);
            if (docent == null) return NotFound();
            return Ok(docent);
        }
    }
}