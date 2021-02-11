using System;
using System.Collections.Generic;

namespace Restaurant.Areas.Admin.Models
{
    public class OrderAdminVM
    {   public int OrderNumber { get; set; }
        public string Username { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> MealsAndQuantities { get; set; }
        public DateTime OrderCreatedAt { get; set; }
    }
}