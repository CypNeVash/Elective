﻿using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for diplaying
    /// all users in database
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = "Admin")]
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