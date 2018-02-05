using BusinessModel;
using System;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for viewing all facultatives and register to them
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Student)]
    public class StudentManagerController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IStudentService _studentService;

        public StudentManagerController(IAccountService accountService, IStudentService studentService)
        {
            _accountService = accountService;
            _studentService = studentService;
        }

        /// <summary>
        /// Viewing all facultatives
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return View(_studentService.GetAllFacultative());
        }

        /// <summary>
        /// Registration for facultative
        /// </summary>
        /// <param name="id">id facultative</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult RegisterFacultative(Guid id)
        {
            Account account = _accountService.GetAccount(User.Identity.Name);

            Student student = _studentService.GetStudent(account);
            if (_studentService.RegToFacultative(student, id))
                return RedirectToAction("Index", "StudentFacultatives");
            return RedirectToAction("Index");
        }
    }
}