using Restaurant.Models.DTOs;
using Restaurant.Models.ViewModels.Account;
using Restaurant.Models.ViewModels.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
            //If a user goes to /account, it'll redirect to login
        }

        //GET: /account/account/login
        [HttpGet]
        public ActionResult Login()
        {
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("user-profile");
                //TODO rename user-profile to something different
            }
            //Return view
            return View();
        }



        [HttpPost]
        //POST /account/login
        public ActionResult Login(LoginUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(u => u.Username.Equals(model.Username) &&
                u.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Username or password not recognised");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                //Setting the cookie
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
            }
            return View(model);
        }

        //GET: /account/logout
        [Authorize] //authorize with no parameters goes for all the roles
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

        [Authorize]
        public ActionResult UserNavigationPartial()
        {

            string username = User.Identity.Name;

            UserNavigationPartialVM model;

            using (Db db = new Db())
            {
                UserDTO dto = db.Users.FirstOrDefault(u => u.Username == username);

                //Build model
                model = new UserNavigationPartialVM()
                {
                    FirstName = dto.FirstName,
                    Surname = dto.Surname
                };
            }
            return PartialView(model);
        }

        //GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            string username = User.Identity.Name;

            UserProfileVM model;

            using (Db db = new Db())
            {
                UserDTO dto = db.Users.FirstOrDefault(u => u.Username == username);

                model = new UserProfileVM(dto);
            }

            return View("UserProfile", model);
        }

        //POST: /account/user-profile
        [HttpPost]
        [Authorize]
        [ActionName("user-profile")]
        
        public ActionResult UserProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            //To stop someone just having a space in the password field
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords don't match");
                    return View("UserProfile", model);
                }
            }

            using (Db db = new Db())
            {

                string username = User.Identity.Name;

                //Make sure it's unique
                if (db.Users.Where(u => u.Id != model.Id).Any
                    (u => u.Username == username))
                {
                    ModelState.AddModelError("", "The username" + model.Username + "is already in use");
                    model.Username = "";  //reset username                  
                    return View("UserProfile", model);
                }

                UserDTO dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.Surname = model.Surname;
                dto.Email = model.Email;
                dto.Username = model.Username;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }

                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Details updated";

            return Redirect("~/account/user-profile");
        }


        //GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        //POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            //Check if passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords don't match");
                return View("CreateAccount", model);
            }

            using (Db db = new Db())
            {
                //Check username is unique
                if (db.Users.Any(u => u.Username.Equals(model.Username)))
                {
                    //ModelState.AddModelError("", "Username already exists");
                    ModelState.AddModelError("", "Username " + model.Username + "already exists");
                    model.Username = ""; //Clears username for security
                    return View("CreateAccount", model);
                }

                UserDTO userDto = new UserDTO()
                {
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password
                };

                //Add to DTO
                db.Users.Add(userDto);

                db.SaveChanges();

                //Add to userRolesDTO
                int id = userDto.Id;

                UserRoleDTO userRolesDto = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = 2
                    //2 is a user, 1 is the admin
                };

                db.UserRoles.Add(userRolesDto);
                db.SaveChanges();
            }

            //Create Tempdata message
            TempData["SuccessMessage"] = "Thanks for registering, you can now log in";

            //Redirect
            return Redirect("~/account/login");
        }
    }
}