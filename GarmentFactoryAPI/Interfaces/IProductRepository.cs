using GarmentFactoryAPI.Models;

namespace GarmentFactoryAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProductById(int productId);
        Product GetProductByName(string productName);
        bool HasProduct(int productId);

    }
}
