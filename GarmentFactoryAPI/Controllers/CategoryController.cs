using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Pagination;
using GarmentFactoryAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly DataContext _context;

        public CategoryController(ICategoryRepository categoryRepository, DataContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }


        // Get all categories with pagination
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PagedResult<CategoryDTO>))]
        public IActionResult GetCategories(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allCategories = _categoryRepository.GetCategories();

            var pagedCategories = allCategories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive
                })
                .ToList();

            var totalCategories = allCategories.Count();
            var result = new PagedResult<CategoryDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCategories,
                Items = pagedCategories
            };

            return Ok(result);
        }

        // Get category by ID
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        // Search categories by name
        [HttpGet("search/{categoryName}")]
        [ProducesResponseType(200, Type = typeof(PagedResult<CategoryDTO>))]
        [ProducesResponseType(404)]
        public IActionResult GetCategoriesByName(string categoryName, int pageNumber = 1, int pageSize = 3)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                return BadRequest("Category name cannot be empty.");
            }

            var categories = _categoryRepository.GetCategoriesByName(categoryName);

            if (!categories.Any())
            {
                return NotFound();
            }

            var pagedCategories = categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            var totalCategories = categories.Count();
            var result = new PagedResult<CategoryDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCategories,
                Items = pagedCategories
            };

            return Ok(result);
        }

        // Create a new category
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is invalid.");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                IsActive = true // Automatically set IsActive to true for a new category
            };

            if (!_categoryRepository.CreateCategory(category))
            {
                return StatusCode(500, "Something went wrong.");
            }

            var createdCategoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive
            };

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = createdCategoryDto.Id }, createdCategoryDto);
        }

        // Update an existing category
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Category data is invalid.");
            }

            // Retrieve the existing category from the repository
            var existingCategory = _categoryRepository.GetCategoryById(categoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Update only the Name field of the existing category
            existingCategory.Name = categoryDto.Name;

            // Update the category in the repository
            if (!_categoryRepository.UpdateCategory(existingCategory))
            {
                return StatusCode(500, "Something went wrong.");
            }

            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            // Find the category by id
            var category = _context.Categories.Find(id);

            // If the category is not found, return a 404 Not Found status
            if (category == null)
            {
                return NotFound();
            }

            // Set IsActive to false instead of removing the category
            category.IsActive = false;
            _context.Entry(category).State = EntityState.Modified;

            // Save changes to the database
            _context.SaveChanges();

            // Return a 204 No Content status to indicate success
            return NoContent();
        }


    }
}