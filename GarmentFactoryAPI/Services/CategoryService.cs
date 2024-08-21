using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace GarmentFactoryAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ICollection<CategoryDTO> GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public CategoryDTO GetCategoryById(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null)
            {
                return null;
            }

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public bool CreateCategory(CategoryDTO categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                IsActive = true // Set default value for new categories
            };

            return _categoryRepository.CreateCategory(category);
        }

        public bool UpdateCategory(CategoryDTO categoryDto)
        {
            var category = new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name
                // No need to handle IsActive here; it should not be modified by updates
            };

            return _categoryRepository.UpdateCategory(category);
        }

        public bool DeleteCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategoryById(categoryId);
            if (category == null)
            {
                return false;
            }

            category.IsActive = false; // Soft delete
            return _categoryRepository.UpdateCategory(category);
        }
    }
}
