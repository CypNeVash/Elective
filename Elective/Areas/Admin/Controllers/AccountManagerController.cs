using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using BusinessModel;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Elective
{
    /// <summary>
    /// Controller for diplaying
    /// all users in database
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Admin)]
    public class AccountManagerController : Controller
    {
        public ApplicationUserManager UserManager
        {
            get => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        /// <summary>
        /// Displaying all users
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }
    }
}