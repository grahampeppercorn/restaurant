using System;
using System.Collections.Generic;

namespace Restaurant.Models.ViewModels.Account
{
    public class CustomerOrdersVM
    {
        public int OrderNumber { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> MealsAndQuantities { get; set; }
        public DateTime OrderCreatedAt { get; set; }
    }
}