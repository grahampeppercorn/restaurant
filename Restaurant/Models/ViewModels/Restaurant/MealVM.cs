using Restaurant.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Models.ViewModels.Restaurant
{
    public class MealVM
    {
        public MealVM()
        {

        }

        public MealVM(MealDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Url = row.Url;
            Description = row.Description;
            Price = row.Price;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            Image = row.Image;
            AmountInStock = row.AmountInStock;
        }

        public int Id { get; set; }
        //------------------------------        
        [Required]
        public string Name { get; set; }
        //------------------------------
        public string Url { get; set; }
        //------------------------------
        [Required]
        public string Description { get; set; }
        //------------------------------
        public decimal Price { get; set; }
        //------------------------------
        public string CategoryName { get; set; }
        //------------------------------
        [Required]
        public int CategoryId { get; set; }
        //------------------------------
        public string Image { get; set; }

        public int AmountInStock { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } //use when adding a product to choose category
        //public IEnumerable<string> GalleryImages { get; set; }
        //TODO - Maybe delete
    }
}