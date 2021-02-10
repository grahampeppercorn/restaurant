using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models.ViewModels.Order
{
    public class MealOrderVM
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        //cool feature
        public string Image { get; set; }
    }
}