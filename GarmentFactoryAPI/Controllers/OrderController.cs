using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTOs;
using GarmentFactoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace GarmentFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public OrdersController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Orders
        [HttpGet("GetAllOrder")]
        public ActionResult<IEnumerable<OrderDTO>> GetOrders()
        {
            var orders = _context.Orders
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.User.Id,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDTO
                    {
                        Id = od.Id,
                        Quantity = od.Quantity,
                        ProductId = od.ProductId,
                      
                    }).ToList()
                }).ToList();

            return orders;
        }

        // GET: api/Orders/5
        [HttpGet("GetOrderById{id}")]
        public ActionResult<OrderDTO> GetOrder(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.User.Id,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDTO
                    {
                        Id = od.Id,
                        Quantity = od.Quantity,
                        ProductId = od.ProductId,
                       
                    }).ToList()
                }).FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        [HttpPost("CreateOrder")]
        public ActionResult<OrderDTO> PostOrder(CreateOrderDTO orderDto)
        {
            var order = new Order
            {
                OrderDate = orderDto.OrderDate,
                TotalPrice = orderDto.TotalPrice,
                User = _context.Users.Find(orderDto.UserId),
                OrderDetails = orderDto.OrderDetails.Select(odDto => new OrderDetail
                {
                    Quantity = odDto.Quantity,
                    ProductId = odDto.ProductId
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
             // Return the ID of the created order

            return CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);
        }

        // PUT: api/Orders/5
        [HttpPut("UpdateOrder/{id}")]
        public IActionResult PutOrder(int id, OrderDTO orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderDate = orderDto.OrderDate;
            order.TotalPrice = orderDto.TotalPrice;
            order.User = _context.Users.Find(orderDto.UserId);
            order.OrderDetails = orderDto.OrderDetails.Select(odDto => new OrderDetail
            {
                Id = odDto.Id,
                Quantity = odDto.Quantity,
                ProductId = odDto.ProductId,
                OrderId = order.Id
            }).ToList();

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }
    }
}