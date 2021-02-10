using Restaurant.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models.ViewModels.Restaurant
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }

        public CategoryVM(CategoryDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Url = row.Url;            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        
    }
}