using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace GarmentFactoryAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context;
        public ProductService(IProductRepository productRepository, DataContext context)
        {
            _context = context;
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

            if (product == null || !product.IsActive) 
                return null;
            
            //map tới thuộc tính của ProductDTO
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

        public PagedResult<ProductDTO> GetProductsByName(string productName, int pageNumber, int pageSize)
        {
            var allProduct = _productRepository.GetProductsByName(productName).Where(p => p.IsActive).ToList();
            if (!allProduct.Any())
                return null;

            var pagedProducts = allProduct
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
                    IsActive = p.IsActive
                })
                .ToList();

            var totalProducts = allProduct.Count();
             return new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };
        }

        public PagedResult<ProductDTO> GetProductsOfCategory(int categoryId, int pageNumber, int pageSize)
        {
            var products = _productRepository.GetProductsOfCategory(categoryId).Where(p => p.IsActive).ToList();

            if (!products.Any())
                return null;

            //map tới thuộc tính của ProductDTO
            var pagedProducts = products
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
                }).ToList();

            var totalProducts = products.Count();
            return new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };
        }

        public ProductDTO CreateProduct(ProductDTO productDto)
        {
            var category = _context.Categories.Find(productDto.CategoryId);
            var user = _context.Users.Find(productDto.UserId);

            if (category == null)
                throw new ArgumentException("ID category is invalid.");
            if (user == null)
                throw new ArgumentException("ID user is invalid.");

            var product = new Product
            {
                Name = productDto.Name,
                Code = productDto.Code,
                Price = productDto.Price,
                Category = category,
                User = user,
                IsActive = true,
            };

            if (!_productRepository.CreateProduct(product))
                throw new Exception("Something went wrong.");

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                CategoryId = product.Category.Id,
                UserId = product.User.Id,
                IsActive = product.IsActive,
            };
        }

        public bool UpdateProduct(int productId, ProductDTO productDto)
        {
            if (productDto == null || productId != productDto.Id)
                throw new ArgumentException("Product data is invalid.");

            var existingProduct = _productRepository.GetProductById(productId);

            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            var category = _context.Categories.Find(productDto.CategoryId);
            var user = _context.Users.Find(productDto.UserId);

            if (category == null)
                throw new ArgumentException("ID category is invalid.");
            if (user == null)
                throw new ArgumentException("ID user is invalid.");

            existingProduct.Name = productDto.Name;
            existingProduct.Code = productDto.Code;
            existingProduct.Price = productDto.Price;
            existingProduct.Category = category;
            existingProduct.User = user;
            existingProduct.IsActive = productDto.IsActive;

            if (!_productRepository.UpdateProduct(existingProduct))
                throw new Exception("Something went wrong.");

            return true;
        }

        public bool DeleteProduct(int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            product.IsActive = false;

            if (!_productRepository.UpdateProduct(product))
                throw new Exception("Something went wrong.");

            return true;
        }
    }
}
