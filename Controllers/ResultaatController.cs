using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public ResultaatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Resultaat>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Docent"))
            {
                return Ok(await _context.Resultaten.ToListAsync());
            }

            var eigenResultaten = await _context.Resultaten
                .Where(r => r.UserId == userId)
                .ToListAsync();
            return Ok(eigenResultaten);
        }
    }
}