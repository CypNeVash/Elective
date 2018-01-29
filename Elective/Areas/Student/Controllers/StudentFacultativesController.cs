using BusinessModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for displaying all register facultatives
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = "Student")]
    public class StudentFacultativesController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;

        public StudentFacultativesController(IAccountService accountService, IStudentService studentService)
        {
            _accountService = accountService;
            _studentService = studentService;
        }

        /// <summary>
        /// Displaying all register facultatives
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            Account account = _accountService.GetAccount(User.Identity.Name);
            Student student = _studentService.GetStudent(account);

            return View(student.RegistrFacultatives.OrderBy(s=>s.Status));
        }
    }
}