using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Restaurant.Models.DTOs
{
    public class Db : DbContext
    {
        public DbSet<CategoryDTO> Categories { get; set; }
        public DbSet<MealDTO> Meals { get; set; }
    }
}