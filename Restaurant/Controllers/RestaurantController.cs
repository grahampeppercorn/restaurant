using Restaurant.Models.DTOs;
using Restaurant.Models.ViewModels.Restaurant;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()        {
            
            List<CategoryVM> categoryVMList;
            
            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                    .ToArray()                    
                    .Select(c => new CategoryVM(c))
                    .ToList();
            }           
            return PartialView(categoryVMList);
        }

        // GET: /restaurant/category/name
        public ActionResult Category(string name)
        {           
            List<MealVM> mealVMList;

            using (Db db = new Db())
            {                
                CategoryDTO categoryDTO = db.Categories
                    .Where(c => c.Url == name)
                    .FirstOrDefault();

                int categoryId = categoryDTO.Id;
               
                mealVMList = db.Meals.ToArray()
                    .Where(p => p.CategoryId == categoryId)
                    .Select(p => new MealVM(p))
                    .ToList();                

                var mealCategory = db.Meals
                    .Where(c => c.CategoryId == categoryId)
                    .FirstOrDefault();

                ViewBag.CategoryName = mealCategory.CategoryName;
                                 
            }            
            return View(mealVMList);
        }

        // GET: /restaurant/meal-details/name
        [ActionName("meal-details")]  //Makes the url /meal-details rather than /mealdetails
        public ActionResult MealDetails(string name)
        {           
            MealVM model;
            
            int id = 0;

            using (Db db = new Db())
            {                
                if (!db.Meals.Any(p => p.Url.Equals(name)))
                {
                    return RedirectToAction("Index", "Restaurant");// index method of the restaurant controller
                }
              
                MealDTO dto;

                dto = db.Meals
                    .Where(p => p.Url == name)
                    .FirstOrDefault();
              
                id = dto.Id;
              
                model = new MealVM(dto);
            }            
            return View("MealDetails", model);
        }
    }
}