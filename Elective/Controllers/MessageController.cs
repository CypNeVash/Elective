using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for displaying some information
    /// </summary>
    public class MessageController : Controller
    {
        public ActionResult Index(string message, string type)
        {
            ViewBag.Type = type;
            ViewBag.Message = message;
            return View();
        }
    }
}