using GarmentFactoryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using GarmentFactoryAPI.Models;
using GermentFactory.Services;

namespace GarmentFactoryAPI.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("api/Category/GetCategories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        [HttpGet]
        [Route("api/Category/GetCategoryById")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetById1(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Route("api/Category/CreateCategory")]
        [ProducesResponseType(201, Type = typeof(Category))]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null");
            }

            var result = await _categoryService.Save(category);
            if (result.Code != Const.SUCCESS_CREATE_CODE)
            {
                return StatusCode(500, result.Message);
            }

            var createdCategory = result.Data as Category;
            if (createdCategory == null)
            {
                return StatusCode(500, "Error creating category");
            }

            return CreatedAtAction("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
        }


        [HttpPut]
        [Route("api/Category/UpdateCategory")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public async Task<ActionResult<Category>> UpdateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null");
            }

            var updatedCategory = await _categoryService.Update(category);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
        }

        [HttpDelete]
        [Route("api/Category/DeleteCategory")]
        [ProducesResponseType(204)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var deletedCategory = await _categoryService.DeleteById1(id);
            if (deletedCategory.Code == Const.SUCCESS_DELETE_CODE)
            {
                return Ok(deletedCategory.Message);
            }

            return NotFound(deletedCategory.Message);
        }
    }
}
