using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Pagination;
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
    [ProducesResponseType(200, Type = typeof(PagedResult<AssemblyLineDTO>))]
    public async Task<IActionResult> GetAssemblyLines(int pageNumber = 1, int pageSize = 3)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0.");
        }

        var allAssemblyLines = await _context.AssemblyLines.ToListAsync();

        // Pagination
        var pagedAssemblyLines = allAssemblyLines
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(al => new AssemblyLineDTO
            {
                Id = al.Id,
                StartDate = al.StartDate,
                EndDate = al.EndDate,
                OrderDetailId = al.OrderDetailId,
                TaskProductId = al.TaskProductId,
                UserId = al.UserId
            })
            .ToList();

        var totalAssemblyLines = allAssemblyLines.Count();
        var result = new PagedResult<AssemblyLineDTO>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalAssemblyLines,
            Items = pagedAssemblyLines
        };

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(result);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAssemblyLine(int id, AssemblyLineDTO assemblyLineDto)
    {
        if (id != assemblyLineDto.Id)
        {
            return BadRequest();
        }

        // Map DTO to Entity
        var assemblyLine = new AssemblyLine
        {
            Id = assemblyLineDto.Id,
            StartDate = assemblyLineDto.StartDate,
            EndDate = assemblyLineDto.EndDate,
            OrderDetailId = assemblyLineDto.OrderDetailId,
            TaskProductId = assemblyLineDto.TaskProductId,
            UserId = assemblyLineDto.UserId
        };

        _context.Entry(assemblyLine).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AssemblyLineExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/AssemblyLines
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
