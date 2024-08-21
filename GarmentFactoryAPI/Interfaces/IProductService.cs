using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Pagination;

namespace GarmentFactoryAPI.Interfaces
{
    public interface IProductService
    {
        PagedResult<ProductDTO> GetPagedProducts(int pageNumber, int pageSize);
        ProductDTO GetProductById(int productId);
        PagedResult<ProductDTO> GetAllProductsFromData(int pageNumber, int pageSize);

    }
}
