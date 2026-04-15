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

        [HttpGet]
        public async Task<ActionResult<List<Oefening>>> GetAll()
        {
            // return await _context.Oefeningen.ToListAsync();
            var oefeningen = await _context.Oefeningen.ToListAsync();
            return Ok(oefeningen);
        }

    }
}