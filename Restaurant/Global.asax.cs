using Restaurant.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Restaurant
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {            
            if (User == null)
            {
                return;
            }
           
            string username = Context.User.Identity.Name;

            //Declare array of roles
            string[] userRoles = null;

            using (Db db = new Db())
            {
                //Populate roles
                UserDTO dto = db.Users.FirstOrDefault(u => u.Username == username);

                userRoles = db.UserRoles.Where(r => r.UserId == dto.Id).Select(r => r.Role.Name).ToArray();
            }

            //Build IPrincipal object
            IIdentity identityOfUser = new GenericIdentity(username);
            IPrincipal objNewUser = new GenericPrincipal(identityOfUser, userRoles);

            //Update Context.User
            Context.User = objNewUser;
        }
    }
}
