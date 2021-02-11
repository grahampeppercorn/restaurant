using Restaurant.Models.DTOs;
using Restaurant.Models.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class MealOrderController : Controller
    {
        // GET: MealOrder
        public ActionResult Index()
        {
            var order = Session["order"] as List<MealOrderVM> ?? new List<MealOrderVM>();

            if (order.Count == 0 || Session["order"] == null)
            {
                ViewBag.Message = "You don't have any meals in your order";
                return View();
            }

            //Calculate total and save to ViewBag
            decimal total = 0m; //m makes it a decimal

            foreach (var item in order)
            {
                total += item.Total;
            }

            ViewBag.OverallTotal = total;

            return View(order);
        }

        public ActionResult OrderPartial()
        {
            MealOrderVM model = new MealOrderVM();

            int quantity = 0;

            decimal price = 0m;

            if (Session["order"] != null)
            {
                var list = (List<MealOrderVM>)Session["order"];

                foreach (var item in list)
                {
                    //BUGFIX forgot this bit so it was resetting to empty on refresh
                    quantity += item.Quantity;
                    price += item.Quantity * item.Price;
                }
                model.Quantity = quantity;
                model.Price = price;
            }
            else
            {//Or set quantity and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }
            return PartialView(model);
        }

        public ActionResult AddToOrderPartial(int id)
        {

            List<MealOrderVM> order = Session["order"] as List<MealOrderVM> ??
                new List<MealOrderVM>();

            MealOrderVM model = new MealOrderVM();

            using (Db db = new Db())
            {
                MealDTO meal = db.Meals.Find(id);

                //Check if the meal is already in the order
                var mealsInOrder = order.FirstOrDefault(c => c.MealId == id);

                //If not in order, add new one
                if (mealsInOrder == null)
                {
                    order.Add(new MealOrderVM()
                    {
                        MealId = meal.Id,
                        MealName = meal.Name,
                        Quantity = 1,
                        Price = meal.Price,
                        Image = meal.Image
                    });
                }
                else
                {
                    // If it is, then increment it but not over 5                    

                    if (mealsInOrder.Quantity < 5)
                    {
                        mealsInOrder.Quantity++;
                    }
                }
            }

            //Get total quantity and price and add it to the model
            int quantity = 0;
            decimal price = 0m;

            foreach (var item in order)
            {
                quantity += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = quantity;
            model.Price = price;

            Session["order"] = order;

            return PartialView(model);
        }

        //GET /mealorder/Increase
        public JsonResult Increase(int mealId)
        {
            List<MealOrderVM> order = Session["order"] as List<MealOrderVM>;

            using (Db db = new Db())
            {
                MealOrderVM model = order.FirstOrDefault(m => m.MealId == mealId);

                //Increase               

                if (model.Quantity < 5)
                {
                    model.Quantity++;
                }

                //Store the needed data
                var result = new { quantity = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET /MealOrder/Decrease
        public ActionResult Decrease(int mealId)
        {
            List<MealOrderVM> order = Session["order"] as List<MealOrderVM>;

            using (Db db = new Db())
            {
                MealOrderVM model = order.FirstOrDefault(c => c.MealId == mealId);

                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    order.Remove(model);
                }

                var result = new { quantity = model.Quantity, price = model.Price };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //GET /MealOrder/Remove

        public void Remove(int mealId)
        {
            List<MealOrderVM> order = Session["order"] as List<MealOrderVM>;

            using (Db db = new Db())
            {
                MealOrderVM model = order.FirstOrDefault(c => c.MealId == mealId);

                order.Remove(model);
            }
        }
        // POST: /MealOrder/BuyNow
        public void BuyNow()
        {

            List<MealOrderVM> order = Session["order"] as List<MealOrderVM>;

            string username = User.Identity.Name;

            using (Db db = new Db())
            {
                OrderDTO orderDTO = new OrderDTO();

                var query = db.Users.FirstOrDefault(u => u.Username == username);
                int userId = query.Id;

                orderDTO.UserId = userId;
                orderDTO.OrderCreatedAt = DateTime.Now;

                db.Orders.Add(orderDTO);

                db.SaveChanges();

                int orderId = orderDTO.OrderId;

                OrderDetailsDTO orderDetailsDTO = new OrderDetailsDTO();

                // Loop through the order
                foreach (var item in order)
                {
                    orderDetailsDTO.OrderId = orderId;
                    orderDetailsDTO.UserId = userId;
                    orderDetailsDTO.MealId = item.MealId;
                    orderDetailsDTO.Quantity = item.Quantity;

                    db.OrderDetails.Add(orderDetailsDTO);

                    db.SaveChanges();
                }
            }
            //Reset session
            Session["order"] = null;
        }
    }
}