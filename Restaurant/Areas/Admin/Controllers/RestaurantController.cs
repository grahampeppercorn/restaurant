using PagedList;
using Restaurant.Areas.Admin.Models;
using Restaurant.Models.DTOs;
using Restaurant.Models.ViewModels.Restaurant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurantController : Controller
    {
        // GET: Admin/Restaurant/Categories
        public ActionResult Categories()
        {
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {

                categoryVMList = db.Categories
                    .ToArray()
                    .Select(c => new CategoryVM(c))
                    .ToList();
            }
            return View(categoryVMList);
        }

        [HttpGet]
        // GET: Admin/Restaurant/AddMeal
        public ActionResult AddMeal()
        {            
            MealVM model = new MealVM();
            
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories
                    .ToList(), "Id", "Name"); //get the categories
            }           
            return View(model);
        }

        [HttpPost]
        // POST: Admin/Restaurant/AddMeal
        public ActionResult AddMeal(MealVM model, HttpPostedFileBase file)
        {            
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories
                        .ToList(), "Id", "Name");

                    return View(model);
                }
            }
                        
            using (Db db = new Db())
            {
                if (db.Meals.Any(p => p.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories
                        .ToList(), "Id", "Name");
                    ModelState.AddModelError("", "Meal name already in use");
                    return View(model);
                }
            }           
            int id;

           
            using (Db db = new Db())
            {
                MealDTO meal = new MealDTO();

                meal.Name = model.Name;
                meal.Url = model.Name.Replace(" ", "-").ToLower();
                meal.Description = model.Description;
                meal.Price = model.Price;
                meal.CategoryId = model.CategoryId;

                //Get the category name
                CategoryDTO categoryDTO = db.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                meal.CategoryName = categoryDTO.Name;

                db.Meals.Add(meal);
                db.SaveChanges();

                //Get the Id
                id = meal.Id;
            }           

            #region Upload image

            // Create folder structure
            var imagesFolder = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var folderPath1 = Path.Combine(imagesFolder.ToString(), "Meals");
            var folderPath2 = Path.Combine(imagesFolder.ToString(), "Meals\\" + id.ToString());
            //concatenate the id to it
            var folderPath3 = Path.Combine(imagesFolder.ToString(), "Meals\\" + id.ToString() + "\\Thumbs");


            if (!Directory.Exists(folderPath1))
                Directory.CreateDirectory(folderPath1);

            if (!Directory.Exists(folderPath2))
                Directory.CreateDirectory(folderPath2);

            if (!Directory.Exists(folderPath3))
                Directory.CreateDirectory(folderPath3);           

            // Check if file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                // Get file extension
                string fileExtension = file.ContentType.ToLower();

                // Verify file extension
                if (fileExtension != "image/jpg" &&
                    fileExtension != "image/jpeg" &&                    
                    fileExtension != "image/gif" &&                    
                    fileExtension != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Invalid file extension, image not uploaded");
                        return View(model);
                    }
                }

                //Init image name
                string image = file.FileName;

                //Save image name to DTO
                using (Db db = new Db())
                {
                    MealDTO dto = db.Meals.Find(id);
                    dto.Image = image;

                    db.SaveChanges();
                }

                //Set original and thumbnail image paths
                var path2 = string.Format("{0}\\{1}", folderPath2, image);
                var path3 = string.Format("{0}\\{1}", folderPath3, image);

                //Save original image
                file.SaveAs(path2);

                //Create and save thumbnail
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path3);
            }
            #endregion

            //Redirect
            return RedirectToAction("AddMeal");
        }

        // GET: Admin/Restaurant/Meals
        public ActionResult Meals(int? page, int? categoryId)
        {           
            List<MealVM> listOfMealVM;
            
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {                
                listOfMealVM = db.Meals.ToArray()
                    .Where(m => categoryId == null || categoryId == 0 || m.CategoryId == categoryId)
                    .Select(m => new MealVM(m))
                    .ToList();
                                
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                                
                ViewBag.SelectedCategory = categoryId.ToString(); //might not be needed, maybe remove
            }            
            var onePageOfProducts = listOfMealVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            
            return View(listOfMealVM);
        }

        // GET: Admin/Shop/EditProduct
        [HttpGet]
        public ActionResult EditMeal(int id)
        {
            MealVM model;

            using (Db db = new Db())
            {               
                MealDTO dto = db.Meals.Find(id);
                
                if (dto == null)
                {
                    return Content("Meal not found");
                }
                
                model = new MealVM(dto);
               
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");

                //TODO Delete
                ////Get all the gallery images
                //model.GalleryImages = Directory
                //    .EnumerateFiles(Server
                //    .MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                //    .Select(f => Path.GetFileName(f));
            }
            return View(model);
        }


        /// 
        /// ////////////////////////
        ///   //TODO add Edit and delete methods if time
        ///
        /// 


        // POST: Admin/Restaurant/Orders
        public ActionResult Orders()
        {
            List<OrderAdminVM> orderAdmin = new List<OrderAdminVM>();

            using (Db db = new Db())
            {                
                List<OrderVM> orders = db.Orders
                    .ToArray()
                    .Select(o => new OrderVM(o))
                    .ToList();

                //Loop through list of OrderVM
                foreach (var order in orders)
                {
                    //init product dictionary
                    Dictionary<string, int> mealsAndQuantities = new Dictionary<string, int>();
                                        
                    decimal total = 0m;

                    //Init list of orderdetails dtos
                    List<OrderDetailsDTO> orderDetailsList = db.OrderDetails
                        .Where(o => o.OrderId == order.OrderId)
                        .ToList();
                                       
                    UserDTO user = db.Users
                        .Where(u => u.Id == order.UserId)
                        .FirstOrDefault();

                    string username = user.Username;

                    //loop through list of orderdetails dtos
                    foreach (var orderDetails in orderDetailsList)
                    {                        
                        MealDTO meal = db.Meals
                            .Where(p => p.Id == orderDetails.MealId)
                            .FirstOrDefault();

                        decimal price = meal.Price;
                        string mealName = meal.Name;
                        
                        mealsAndQuantities.Add(mealName, orderDetails.Quantity);
                       
                        total += orderDetails.Quantity * price;
                    }

                    //add to orderAdmin list
                    orderAdmin.Add(new OrderAdminVM()
                    {
                        OrderNumber = order.OrderId,
                        Username = username,
                        Total = total,
                        MealsAndQuantities = mealsAndQuantities,
                        OrderCreatedAt = order.OrderCreatedAt
                    });
                }
            }         
            return View(orderAdmin);
        }
    }
}