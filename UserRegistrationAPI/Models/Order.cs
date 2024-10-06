using System;
using System.Collections.Generic;

namespace UserRegistrationAPI.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // e.g., Processing, Dispatched, In Transit, Delivered, Canceled
        public string ShippingAddress { get; set; }
        public string ContactNumber { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
