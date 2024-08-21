using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GarmentFactoryAPI.Repository
{
    public class TaskProductRepository : ITaskProductRepository
    {
        private readonly DataContext _context;

        public TaskProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<TaskProduct> GetTaskProducts()
        {
            return _context.TaskProducts
                .Include(tp => tp.User)
                .Include(tp => tp.AssemblyLines)
                .OrderBy(tp => tp.Id)
                .ToList();
        }

        public TaskProduct GetTaskProductById(int taskProductId)
        {
            return _context.TaskProducts
                .Include(tp => tp.User)
                .Include(tp => tp.AssemblyLines)
                .FirstOrDefault(tp => tp.Id == taskProductId);
        }

        public ICollection<TaskProduct> GetTaskProductsByName(string taskProductName)
        {
            return _context.TaskProducts
                .Include(tp => tp.User)
                .Include(tp => tp.AssemblyLines)
                .Where(tp => EF.Functions.Like(tp.Name.ToLower(), $"%{taskProductName.ToLower()}%") && tp.IsActive)
                .ToList();
        }

        public bool HasTaskProduct(int taskProductId)
        {
            return _context.TaskProducts.Any(tp => tp.Id == taskProductId);
        }

        public bool HasTaskProduct(string taskProductName)
        {
            return _context.TaskProducts.Any(tp => tp.Name == taskProductName);
        }

        public bool CreateTaskProduct(TaskProduct taskProduct)
        {
            _context.TaskProducts.Add(taskProduct);
            return Save();
        }

        public bool UpdateTaskProduct(TaskProduct taskProduct)
        {
            _context.TaskProducts.Update(taskProduct);
            return Save();
        }

        public bool DeleteTaskProduct(TaskProduct taskProduct)
        {
            taskProduct.IsActive = false;
            _context.TaskProducts.Update(taskProduct);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
