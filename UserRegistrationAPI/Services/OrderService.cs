using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(string id, Order order);
        Task<bool> CancelOrderAsync(string id);
        Task<Order> GetOrderByIdAsync(string id);
        Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId);
    }

    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Id = Guid.NewGuid().ToString();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Processing";
            _orders.Add(order);
            return await Task.FromResult(order);
        }

        public async Task<Order> UpdateOrderAsync(string id, Order order)
        {
            var existingOrder = await GetOrderByIdAsync(id);
            if (existingOrder == null || existingOrder.Status != "Processing")
            {
                return null;
            }

            existingOrder.ShippingAddress = order.ShippingAddress;
            existingOrder.ContactNumber = order.ContactNumber;
            existingOrder.Items = order.Items;
            return existingOrder;
        }

        public async Task<bool> CancelOrderAsync(string id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order == null || order.Status != "Processing")
            {
                return false;
            }

            order.Status = "Canceled";
            return true;
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            return await Task.FromResult(_orders.Find(o => o.Id == id));
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await Task.FromResult(_orders.FindAll(o => o.CustomerId == customerId));
        }
    }
}
