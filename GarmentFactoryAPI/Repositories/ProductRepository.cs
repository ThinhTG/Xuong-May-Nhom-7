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
                .OrderBy(p => p.Id)
                .ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.Id == productId)
                .FirstOrDefault();
        }

        public ICollection<Product> GetProductsByName(string productName)
        {
            return _context.Products
                .Include (p => p.Category)
                .Include (p => p.User)
                .Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{productName.ToLower()}%"))
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

        public bool CreateProduct(int categoryId, Product product)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
