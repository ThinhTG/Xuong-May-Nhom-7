using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;

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
            return _context.Products.OrderBy(p => p.Id).ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Where(p => p.Id == productId).FirstOrDefault();
        }

        public Product GetProductByName(string productName)
        {
            return _context.Products.Where(p => p.Name == productName).FirstOrDefault();
        }

        public bool HasProduct(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }
    }
}
