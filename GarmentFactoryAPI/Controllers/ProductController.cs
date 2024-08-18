using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Lấy tất cả các product
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public IActionResult GetProducts()
        {
            //map tới thuộc tính của ProductDTO
            var product = _productRepository.GetProducts()
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(product);
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return BadRequest("Product name cannot be empty.");

            var products = _productRepository.GetProductsByName(productName);

            if (!products.Any())
                return NotFound();

            //map tới thuộc tính của ProductDTO
            var productDtos = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Price = p.Price,
                CategoryId = p.Category.Id,
                UserId = p.User.Id
            }).ToList();

            return Ok(productDtos);
        }

        //Lấy product dựa trên category Id
        [HttpGet("categoryId/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsOfCategory(int categoryId)
        {
            

            var products = _productRepository.GetProductsOfCategory(categoryId);

            if (!products.Any())
                return NotFound();
            //map tới thuộc tính của ProductDTO
            var productDtos = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Price = p.Price,
                CategoryId = p.Category.Id,
                UserId = p.User.Id
            }).ToList();

            return Ok(productDtos);
        }
    }

}
