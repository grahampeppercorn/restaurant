using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models.DTOs
{
    [Table("tblMeals")]
    public class MealDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public int AmountInStock { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryDTO Category { get; set; } // virtual used for lazyloading
    }
}