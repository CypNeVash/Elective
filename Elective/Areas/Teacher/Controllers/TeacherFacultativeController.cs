using BusinessModel;
using System;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for displaying all facultative
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Teacher)]
    public class TeacherFacultativeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITeacherService _teacherService;

        public TeacherFacultativeController(IAccountService accountService, ITeacherService teacherService)
        {
            _accountService = accountService;
            _teacherService = teacherService;
        }

        /// <summary>
        /// Controller for displaying all facultative
        /// </summary>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return View(_teacherService.GetTeacher(_accountService.GetAccount(User.Identity.Name)));
        }


        [HttpGet]
        public ActionResult WhatchLog(Guid id)
        {
            Facultative facultative = _teacherService.GetFacultative(new Facultative() { Id = id });

            return View(facultative);
        }

    }
}