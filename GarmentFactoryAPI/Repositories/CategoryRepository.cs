using GarmentFactoryAPI.Data;
using System.Collections.Generic;
using GarmentFactoryAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace GarmentFactoryAPI.Repositories

{
    public class CategoryRepository : GenericRepository<Category>
    {
        private new readonly DataContext _context;
        public CategoryRepository(DataContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(u => u.Id == id);
        }


    }
}
