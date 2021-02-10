using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Restaurant.Models.DTOs
{
    [Table("tblOrderDetails")]
    public class OrderDetailsDTO
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int MealId { get; set; }
        public int Quantity { get; set; }

        //Foreign keys

        [ForeignKey("OrderId")]
        public virtual OrderDTO Orders { get; set; }

        [ForeignKey("UserId")]
        public virtual UserDTO Users { get; set; }

        [ForeignKey("MealId")]
        public virtual MealDTO Meals { get; set; }
    }
}