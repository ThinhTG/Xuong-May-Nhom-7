using GarmentFactoryAPI.Models;

namespace GarmentFactoryAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        ICollection<Product> GetAllProductsFromData();
        Product GetProductById(int productId);
        ICollection<Product> GetProductsByName(string productName);
        ICollection<Product> GetProductsOfCategory(int categoryId);

        bool HasProduct(int productId);
        bool HasProduct(string productName);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(Product product);
        bool Save();

    }
}
