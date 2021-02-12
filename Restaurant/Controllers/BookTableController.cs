using Restaurant.Models.DTOs;
using Restaurant.Models.ViewModels.Restaurant;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class BookTableController : Controller
    {
        // GET: BookTable/Book-Table
        [ActionName("book-table")]
        [HttpGet]
        public ActionResult BookTable()
        {
            return View("BookTable");
        }

        //POST: BookTable/Book-Table
        [ActionName("book-table")]
        [HttpPost]

        public ActionResult CreateAccount(BookTableVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("BookTable", model);
            }

            using (Db db = new Db())
            {
                BookTableDTO bookTableDto = new BookTableDTO()
                {
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    NumberOfPeople = model.NumberOfPeople,
                    DateForBooking = model.DateForBooking,
                    TimeOfDay = model.TimeOfDay
                };

                db.BookTables.Add(bookTableDto);
                db.SaveChanges();
            }    

            //Incomplete but out of time.  Finish or remove from final build
           
            return Redirect("~/");
        }
    }
}