using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GarmentFactoryAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.IsActive == true)
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.Id == productId && p.IsActive == true)
                .FirstOrDefault();
        }

        public ICollection<Product> GetProductsByName(string productName)
        {
            return _context.Products
                .Include (p => p.Category)
                .Include (p => p.User)
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{productName.ToLower()}%") && p.IsActive == true)
                .ToList();
        }

        public bool HasProduct(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }
        public bool HasProduct(string productName)
        {
            return _context.Products.Any(p => p.Name == productName);
        }
        public ICollection<Product> GetProductsOfCategory(int categoryId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include (p => p.User)
                .Where(p => p.Category.Id == categoryId).ToList();
        }

        public bool CreateProduct(Product product)
        {
            _context.Products.Add(product);
            return Save();
        }

        public bool UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public ICollection<Product> GetAllProductsFromData()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .OrderBy(p => p.Id)
                .ToList();
        }
    }
}
