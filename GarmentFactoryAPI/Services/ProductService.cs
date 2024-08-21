using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Pagination;

namespace GarmentFactoryAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public PagedResult<ProductDTO> GetAllProductsFromData(int pageNumber, int pageSize)
        {
            var allProducts = _productRepository.GetAllProductsFromData();

            var pagedProducts = allProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Price = p.Price,
                    CategoryId = p.Category.Id,
                    UserId = p.User.Id,
                    IsActive = p.IsActive,
                })
                .ToList();

            var totalProducts = allProducts.Count();

            return new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };
        }

        public PagedResult<ProductDTO> GetPagedProducts(int pageNumber, int pageSize)
        {
            var allProducts = _productRepository.GetProducts();

            var pagedProducts = allProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Price = p.Price,
                    CategoryId = p.Category.Id,
                    UserId = p.User.Id,
                    IsActive = p.IsActive,
                })
                .ToList();

            var totalProducts = allProducts.Count();
            return new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };
        }

        public ProductDTO GetProductById(int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                CategoryId = product.Category.Id,
                UserId = product.User.Id,
                IsActive = product.IsActive
            };
        }
    }
}
