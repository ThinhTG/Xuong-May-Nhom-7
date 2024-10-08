﻿using GarmentFactoryAPI.Data;
using GarmentFactoryAPI.DTO;
using GarmentFactoryAPI.DTOs;
using GarmentFactoryAPI.Models;
using GarmentFactoryAPI.Pagination;
using Microsoft.AspNetCore.Authorization;
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
        // GET: api/Orders/GetAllOrders (With Pagination)
        [HttpGet("GetAllOrdersIsActive")]
        [ProducesResponseType(200, Type = typeof(PagedResult<OrderDTO>))]
        public IActionResult GetOrders(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allOrders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.IsActive) // Only get active orders
                .Select(o => new OrderDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.User.Id,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailSummaryDTO
                    {
                        OrderDetailId = od.Id,
                        Quantity = od.Quantity,
                        ProductId = od.ProductId,
                   
                    }).ToList()
                });

            var pagedOrders = allOrders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalOrders = allOrders.Count();

            var result = new PagedResult<OrderDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalOrders,
                Items = pagedOrders
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        // GET: api/Orders/GetAllOrdersFromData (With Pagination)
        [HttpGet("GetAllOrdersFromData")]
        [ProducesResponseType(200, Type = typeof(PagedResult<OrderDTO>))]
        public IActionResult GetOrder(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var allOrders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Select(o => new OrderDTO
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    UserId = o.User.Id,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailSummaryDTO
                    {
                        OrderDetailId = od.Id,
                        Quantity = od.Quantity,
                        ProductId = od.ProductId,

                    }).ToList()
                });

            var pagedOrders = allOrders
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalOrders = allOrders.Count();

            var result = new PagedResult<OrderDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalOrders,
                Items = pagedOrders
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }


        // GET: api/Orders/GetOrderById/5
        [HttpGet("GetOrderById/{id}")]
        [ProducesResponseType(200, Type = typeof(PagedResult<OrderDetailDTO>))]
        [ProducesResponseType(404)]
        public ActionResult<PagedResult<OrderDetailDTO>> GetOrderById(int id, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.TotalPrice,
                    o.IsActive,
                    UserId = o.User.Id,
                    UserName = o.User.Username, // Assuming you have a Name property in User
                    OrderDetails = o.OrderDetails
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(od => new OrderDetailDTO
                        {
                            OrderDetailId = od.Id,
                            Quantity = od.Quantity,
                            ProductId = od.ProductId,
                            ProductName = od.Product.Name,
                            Price = od.Product.Price,
                            ProductCode = od.Product.Code,
                         
                        }).ToList()
                })
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound("Order not found." );
            }

             if (!order.IsActive)
             {
               return NotFound("The requested order is inactive.");
             }

            var totalOrderDetails = _context.OrderDetails
                .Count(od => od.OrderId == id);

            var result = new PagedResult<OrderDetailDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalOrderDetails,
                Items = order.OrderDetails
            };

            // You can also include the order's status in the response
            var response = new
            {
                OrderId = order.Id,
                order.OrderDate,
                order.TotalPrice,
                order.IsActive,
                order.UserId,
                order.UserName,
                PaginatedOrderDetails = result
            };

            return Ok(response);
        }



        [Authorize(Policy = "RequireStaffRole")]
        // POST: api/Orders
        [HttpPost("CreateOrder")]
        public ActionResult<OrderDTO> PostOrder(CreateOrderDTO orderDto)
        {
            // Kiểm tra nếu OrderDate nhỏ hơn ngày hiện tại
            if (orderDto.OrderDate.Date < DateTime.Now.Date)
            {
                return NotFound("Order date cannot be in the past.");
            }

            var order = new Order
            {
                OrderDate = orderDto.OrderDate,
                TotalPrice = orderDto.TotalPrice,
                User = _context.Users.Find(orderDto.UserId),
                OrderDetails = orderDto.OrderDetails.Select(odDto => new OrderDetail
                {
                    Quantity = odDto.Quantity,
                    ProductId = odDto.ProductId
                }).ToList(),
                IsActive = true  // Đặt IsActive là true mặc định
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderDto);
        }

        [Authorize(Policy = "RequireStaffRole")]
        // PUT: api/Orders/5
        [HttpPut("UpdateOrder/{id}")]
        public ActionResult PutOrder(int id, CreateOrderDTO orderDto)
        {
            // Kiểm tra nếu OrderDate nhỏ hơn ngày hiện tại
            if (orderDto.OrderDate.Date < DateTime.Now.Date)
            {
                return NotFound("Order date cannot be in the past.");
            }

            var order = _context.Orders
                                .Include(o => o.OrderDetails)
                                .FirstOrDefault(o => o.Id == id && o.IsActive);

            if (order == null)
            {
                return NotFound("Order not found or is inactive.");
            }

            // Xóa các OrderDetails hiện có
            _context.OrderDetails.RemoveRange(order.OrderDetails);

            // Cập nhật các trường của đơn hàng
            order.OrderDate = orderDto.OrderDate;
            order.TotalPrice = orderDto.TotalPrice;
            order.User = _context.Users.Find(orderDto.UserId);

            // Thêm các OrderDetails mới từ DTO
            order.OrderDetails = orderDto.OrderDetails.Select(odDto => new OrderDetail
            {
                Quantity = odDto.Quantity,
                ProductId = odDto.ProductId,
                OrderId = order.Id
            }).ToList();

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [Authorize(Policy = "RequireStaffRole")]
        // PUT: api/Orders/UpdateOrderStatus/{id}
        [HttpPut("UpdateOrderStatus/{id}")]
        public ActionResult UpdateOrderStatus(int id, bool isActive)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.IsActive = isActive;

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [Authorize(Policy = "RequireStaffRole")]
        // DELETE: api/Orders/5
        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound("Order not found or is inactive.");
            }

            // Instead of removing the order, set IsActive to false
            order.IsActive = false;
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
    }
}