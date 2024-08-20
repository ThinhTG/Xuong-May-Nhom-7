using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProductController : ControllerBase
    {
        private readonly ITaskProductRepository _taskProductRepository;
        private readonly ITaskProductService _taskProductService;
        private readonly DataContext _context;

        public TaskProductController(ITaskProductRepository taskProductRepository, DataContext context)
        {
            _taskProductRepository = taskProductRepository;
            _context = context;
        }

        // Get all task products with pagination
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PagedResult<TaskProductDTO>))]
        public IActionResult GetTaskProducts(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allTaskProducts = _taskProductRepository.GetTaskProducts();

            var pagedTaskProducts = allTaskProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(tp => new TaskProductDTO
                {
                    Id = tp.Id,
                    Name = tp.Name
                })
                .ToList();

            var totalTaskProducts = allTaskProducts.Count();
            var result = new PagedResult<TaskProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalTaskProducts,
                Items = pagedTaskProducts
            };

            return Ok(result);
        }

        // Get task product by ID
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TaskProductDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetTaskProductById(int id)
        {
            var taskProduct = _taskProductRepository.GetTaskProductById(id);
            if (taskProduct == null)
            {
                return NotFound();
            }

            var taskProductDto = new TaskProductDTO
            {
                Id = taskProduct.Id,
                Name = taskProduct.Name
            };

            return Ok(taskProductDto);
        }

        // Create a new task product
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskProductDTO))]
        [ProducesResponseType(400)]
        public IActionResult CreateTaskProduct([FromBody] TaskProductDTO taskProductDto)
        {
            if (taskProductDto == null)
            {
                return BadRequest("Task product data is invalid.");
            }

            var taskProduct = new TaskProduct
            {
                Name = taskProductDto.Name
            };

            if (!_taskProductRepository.CreateTaskProduct(taskProduct))
            {
                return StatusCode(500, "Something went wrong.");
            }

            var createdTaskProductDto = new TaskProductDTO
            {
                Id = taskProduct.Id,
                Name = taskProduct.Name
            };

            return CreatedAtAction(nameof(GetTaskProductById), new { id = createdTaskProductDto.Id }, createdTaskProductDto);
        }

        // Update an existing task product
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTaskProduct(int id, [FromBody] TaskProductDTO taskProductDto)
        {
            if (taskProductDto == null || id != taskProductDto.Id)
            {
                return BadRequest("Task product data is invalid.");
            }

            var existingTaskProduct = _taskProductRepository.GetTaskProductById(id);
            if (existingTaskProduct == null)
            {
                return NotFound();
            }

            existingTaskProduct.Name = taskProductDto.Name;

            if (!_taskProductRepository.UpdateTaskProduct(existingTaskProduct))
            {
                return StatusCode(500, "Something went wrong.");
            }

            return NoContent();
        }

        // Delete a task product (Set IsActive to false)
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTaskProduct(int id)
        {
            var taskProduct = _context.TaskProducts.Find(id);
            if (taskProduct == null)
            {
                return NotFound();
            }

            taskProduct.IsActive = false;
            _context.Entry(taskProduct).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
