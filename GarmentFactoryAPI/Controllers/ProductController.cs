using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarmentFactoryAPI.Pagination;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context;
        
        public ProductController(IProductRepository productRepository, DataContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        //Lấy tất cả các product đang Active
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PagedResult<ProductDTO>))]
        public IActionResult GetProducts(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allProducts = _productRepository.GetProducts();

            // Thêm phân trang
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
                    UserId = p.User.Id
                })
                .ToList();

            var totalProducts = allProducts.Count();
            var result = new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        //Lấy tất cả các product đang Active
        [HttpGet("allProductFromData")]
        [ProducesResponseType(200, Type = typeof(PagedResult<ProductDTO>))]
        public IActionResult GetAllProductsFromData(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allProducts = _productRepository.GetAllProductsFromData();

            // Thêm phân trang
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
                    UserId = p.User.Id
                })
                .ToList();

            var totalProducts = allProducts.Count();
            var result = new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        //Lấy product theo Id
        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(ProductDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetProductById(int productId)
        {
            if(!_productRepository.HasProduct(productId))
                return NotFound();

            var product = _productRepository.GetProductById(productId);
            
            //map tới thuộc tính của ProductDTO
            var productDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                CategoryId = product.Category.Id,  
                UserId = product.User.Id           
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                return Ok(productDto);
        }

        //Search product theo tên
        [HttpGet("search/{productName}")]
        [ProducesResponseType(200, Type = typeof(PagedResult<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsByName(string productName, int pageNumber = 1, int pageSize = 3)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return BadRequest("Product name cannot be empty.");

            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var products = _productRepository.GetProductsByName(productName);

            if (!products.Any())
                return NotFound();

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
                    UserId = p.User.Id
                })
                .ToList();

            var totalProducts = products.Count();
            var result = new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };

            return Ok(result);
        }

        //Lấy product dựa trên category Id
        [HttpGet("categoryId/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(PagedResult<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsOfCategory(int categoryId, int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var products = _productRepository.GetProductsOfCategory(categoryId);

            if (!products.Any())
                return NotFound();

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
                UserId = p.User.Id
            }).ToList();

            var totalProducts = products.Count();
            var result = new PagedResult<ProductDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalProducts,
                Items = pagedProducts
            };

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDTO))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDTO productDto)
        {         
            if (productDto == null)
                return BadRequest("Data invalid.");

            var category = _context.Categories.Find(productDto.CategoryId);
            var user = _context.Users.Find(productDto.UserId);

            if (category == null)
                return BadRequest("ID category is invalid.");

            if (user == null)
                return BadRequest("ID user is invalid.");

            var product = new Product
            {
                Name = productDto.Name,
                Code = productDto.Code,
                Price = productDto.Price,
                Category = category,
                User = user
            };

            if (!_productRepository.CreateProduct(product))
                return StatusCode(500, "Something went wrong.");

            // Tạo DTO cho sản phẩm đã được tạo
            var createdProductDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                CategoryId = product.Category.Id,
                UserId = product.User.Id
            };

            // Trả về thông tin sản phẩm đã tạo
            return CreatedAtAction(nameof(GetProductById), new { productId = createdProductDto.Id }, createdProductDto);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDTO productDto)
        {
            if (productDto == null || productId != productDto.Id)
                return BadRequest("Product data is invalid.");

            var existingProduct = _productRepository.GetProductById(productId);

            if (existingProduct == null)
                return NotFound();

            var category = _context.Categories.Find(productDto.CategoryId);
            var user = _context.Users.Find(productDto.UserId);

            if (category == null)
                return BadRequest("ID category is invalid.");

            if (user == null)
                return BadRequest("ID user is invalid.");

            existingProduct.Name = productDto.Name;
            existingProduct.Code = productDto.Code;
            existingProduct.Price = productDto.Price;
            existingProduct.Category = category;
            existingProduct.User = user;

            if (!_productRepository.UpdateProduct(existingProduct))
                return StatusCode(500, "Something went wrong.");

            return NoContent();
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            var product = _productRepository.GetProductById(productId);

            if (product == null)
                return NotFound();

            product.IsActive = false;

            if (!_productRepository.UpdateProduct(product))
                return StatusCode(500, "Something went wrong.");

            return NoContent();
        }


    }

}
