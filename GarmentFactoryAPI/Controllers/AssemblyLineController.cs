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
    public async Task<ActionResult<AssemblyLineDTO>> GetAssemblyLine(int id)
    {
        var assemblyLine = await _context.AssemblyLines.FindAsync(id);

        if (assemblyLine == null)
        {
            return NotFound();
        }

        var assemblyLineDTO = new AssemblyLineDTO
        {
            Id = assemblyLine.Id,
            StartDate = assemblyLine.StartDate,
            EndDate = assemblyLine.EndDate,
            OrderDetailId = assemblyLine.OrderDetailId,
            TaskProductId = assemblyLine.TaskProductId,
            UserId = assemblyLine.UserId
        };

        return assemblyLineDTO;
    }



    // POST: api/AssemblyLines
    [HttpPost]
    [HttpPost]
    public async Task<ActionResult<AssemblyLineDTO>> PostAssemblyLine(AssemblyLineDTO assemblyLineDto)
    {
        // Map the DTO to the entity
        var assemblyLine = new AssemblyLine
        {
            StartDate = assemblyLineDto.StartDate,
            EndDate = assemblyLineDto.EndDate,
            OrderDetailId = assemblyLineDto.OrderDetailId,
            TaskProductId = assemblyLineDto.TaskProductId,
            UserId = assemblyLineDto.UserId
        };

        _context.AssemblyLines.Add(assemblyLine);
        await _context.SaveChangesAsync();

        // Map the entity back to the DTO
        var createdAssemblyLineDto = new AssemblyLineDTO
        {
            Id = assemblyLine.Id,
            StartDate = assemblyLine.StartDate,
            EndDate = assemblyLine.EndDate,
            OrderDetailId = assemblyLine.OrderDetailId,
            TaskProductId = assemblyLine.TaskProductId,
            UserId = assemblyLine.UserId
        };

        return CreatedAtAction("GetAssemblyLine", new { id = assemblyLine.Id }, createdAssemblyLineDto);
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
