using AppBackend.Data;
using AppBackend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalleController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalleController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Salle>> GetSalles()
    {
        return await _context.Salles.ToListAsync();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddSalle([FromBody] Salle salle)
    {
        _context.Salles.Add(salle);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSalles), new { id = salle.Id }, salle);
    }
}