// All order processings will be done in this compenet

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistrationAPI.Models;
using UserRegistrationAPI.Services;

namespace UserRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return Ok(createdOrder);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateOrder(string id, Order order)
        {
            var updatedOrder = await _orderService.UpdateOrderAsync(id, order);
            if (updatedOrder == null)
            {
                return BadRequest("Order cannot be updated.");
            }
            return Ok(updatedOrder);
        }

        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(string id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
            {
                return BadRequest("Order cannot be canceled.");
            }
            return Ok("Order canceled successfully.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }
    }
}
