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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public IActionResult GetProducts()
        {
            var product = _productRepository.GetProducts()
                             .Select(p => new ProductDTO
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Code = p.Code,
                                 Price = p.Price
                             })
                             .ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(product);
        }
    }
}
