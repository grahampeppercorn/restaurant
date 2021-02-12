using Restaurant.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models.ViewModels.Restaurant
{
    public class OrderVM
    {       
        public OrderVM(OrderDTO row)
        {
            OrderId = row.OrderId;
            UserId = row.UserId;
            OrderCreatedAt = row.OrderCreatedAt;
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderCreatedAt { get; set; }
    }
}