using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GarmentFactoryAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Id)
                .ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories
                .Where(c => c.Id == categoryId && c.IsActive == true)
                .FirstOrDefault();
        }

        public ICollection<Category> GetCategoriesByName(string categoryName)
        {
            return _context.Categories
                .Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{categoryName.ToLower()}%") && c.IsActive == true)
                .ToList();
        }

        public bool HasCategory(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool HasCategory(string categoryName)
        {
            return _context.Categories.Any(c => c.Name == categoryName);
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public ICollection<Category> GetAllCategoriesFromData()
        {
            return _context.Categories
                .OrderBy(c => c.Id)
                .ToList();
        }
    }
}
