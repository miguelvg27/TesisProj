using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Models.Storage;
using System.Web.Security;
using WebMatrix.WebData;

namespace TesisProj.Areas.Seguridad.Controllers
{
    [Authorize(Roles="admin")]
    public class UserProfileController : Controller
    {
        private TProjContext db = new TProjContext();
        //
        // GET: /Seguridad/UserProfile/

        public ActionResult Index()
        {
            var users = db.UserProfiles.OrderBy(u => u.UserName).ToList();
            return View(users);
        }

        public ActionResult Block(string username, bool policy)
        {
            if (policy && Roles.IsUserInRole(username, "nav"))
            {
                Roles.RemoveUserFromRole(username, "nav");
            }

            if (policy && Roles.IsUserInRole(username, "admin"))
            {
                Roles.RemoveUserFromRole(username, "admin");
            }

            if (!policy && !Roles.IsUserInRole(username, "nav"))
            {
                Roles.AddUserToRole(username, "nav");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Prize(string username, bool policy)
        {
            if (!policy && Roles.IsUserInRole(username, "admin"))
            {
                Roles.RemoveUserFromRole(username, "admin");
            }

            if (policy && !Roles.IsUserInRole(username, "admin"))
            {
                Roles.AddUserToRole(username, "admin");
            }

            return RedirectToAction("Index");
        }

    }
}
