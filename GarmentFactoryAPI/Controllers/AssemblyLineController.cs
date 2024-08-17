using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AssemblyLinesController : ControllerBase
{
    private readonly DataContext _context;

    public AssemblyLinesController(DataContext context)
    {
        _context = context;
    }

    // GET: api/AssemblyLines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssemblyLineDTO>>> GetAssemblyLines()
    {
        var assemblyLines = await _context.AssemblyLines
            .Select(al => new AssemblyLineDTO
            {
                Id = al.Id,
                StartDate = al.StartDate,
                EndDate = al.EndDate,
                OrderDetailId = al.OrderDetailId,
                TaskProductId = al.TaskProductId,
                UserId = al.UserId
            })
            .ToListAsync();

        return Ok(assemblyLines);
    }

    // GET: api/AssemblyLines/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AssemblyLine>> GetAssemblyLine(int id)
    {
        var assemblyLine = await _context.AssemblyLines.FindAsync(id);

        if (assemblyLine == null)
        {
            return NotFound();
        }

        return assemblyLine;
    }

    
    // POST: api/AssemblyLines
    [HttpPost]
    public async Task<ActionResult<AssemblyLine>> PostAssemblyLine(AssemblyLine assemblyLine)
    {
        _context.AssemblyLines.Add(assemblyLine);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAssemblyLine", new { id = assemblyLine.Id }, assemblyLine);
    }

    // DELETE: api/AssemblyLines/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssemblyLine(int id)
    {
        var assemblyLine = await _context.AssemblyLines.FindAsync(id);
        if (assemblyLine == null)
        {
            return NotFound();
        }

        _context.AssemblyLines.Remove(assemblyLine);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AssemblyLineExists(int id)
    {
        return _context.AssemblyLines.Any(e => e.Id == id);
    }
}
