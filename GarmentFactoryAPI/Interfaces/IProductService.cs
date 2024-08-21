using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Pagination;

namespace GarmentFactoryAPI.Interfaces
{
    public interface IProductService
    {
        PagedResult<ProductDTO> GetPagedProducts(int pageNumber, int pageSize);
        ProductDTO GetProductById(int productId);
        PagedResult<ProductDTO> GetAllProductsFromData(int pageNumber, int pageSize);

        PagedResult<ProductDTO> GetProductsByName(string productName, int pageNumber, int pageSize);
        PagedResult<ProductDTO> GetProductsOfCategory(int categoryId, int pageNumber, int pageSize);
        ProductDTO CreateProduct(ProductDTO productDto);
        bool UpdateProduct(int productId, ProductDTO productDto);
        bool DeleteProduct(int productId);
    }
}
