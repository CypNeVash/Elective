using BusinessModel;
using System;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for displaying select facultative
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Student)]
    public class StudentFacultativeController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentFacultativeController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// displaying select facultative
        /// </summary>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(Guid id)
        {
            return View(_studentService.GetFacultative(new Facultative() { Id = id }));
        }
    }
}