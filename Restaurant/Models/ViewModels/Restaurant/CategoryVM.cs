using Restaurant.Models.DTOs;

namespace Restaurant.Models.ViewModels.Restaurant
{
    public class CategoryVM
    {  
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