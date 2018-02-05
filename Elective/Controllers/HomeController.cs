using BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Home controller
    /// </summary>
    [ErrorExeptionFilter]
    public class HomeController : Controller
    {
        /// <summary>
        /// Display general info
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Display contact info
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }
    }
}