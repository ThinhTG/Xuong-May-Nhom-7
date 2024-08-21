using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.Interfaces;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GarmentFactoryAPI.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly DataContext _context;
        private readonly IProductService _productService;

        public ProductController(IProductRepository productRepository, DataContext context, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
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

            var result = _productService.GetPagedProducts(pageNumber, pageSize);

            if (result == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        //Lấy tất cả các product 
        [HttpGet("allProductFromData")]
        [ProducesResponseType(200, Type = typeof(PagedResult<ProductDTO>))]
        public IActionResult GetAllProductsFromData(int pageNumber = 1, int pageSize = 3)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var result = _productService.GetAllProductsFromData(pageNumber, pageSize);

            if (result == null)
                return NotFound();

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

            var product = _productService.GetProductById(productId);

            if(product == null || !product.IsActive)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                return Ok(product);
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

            var products = _productService.GetProductsByName(productName, pageNumber, pageSize);

            if (products == null || !products.Items.Any())
                return NotFound();

            return Ok(products);
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

            var products = _productService.GetProductsOfCategory(categoryId, pageNumber, pageSize);

            if (!products.Items.Any())
                return NotFound();

            return Ok(products);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDTO))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                var createdProduct = _productService.CreateProduct(productDto);
                return CreatedAtAction(nameof(GetProductById), new { productId = createdProduct.Id }, createdProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong.");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDTO productDto)
        {
            try
            {
                if (_productService.UpdateProduct(productId, productDto))
                    return NoContent();
                else
                    return StatusCode(500, "Something went wrong.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong.");
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                if (_productService.DeleteProduct(productId))
                    return NoContent();
                else
                    return StatusCode(500, "Something went wrong.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong.");
            }
        }


    }

}
