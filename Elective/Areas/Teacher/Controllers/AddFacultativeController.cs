using BusinessModel;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// controller for creating 
    /// facultative by teacher
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Teacher)]
    public class AddFacultativeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITeacherService _teacherService;

        public AddFacultativeController(IAccountService accountService, ITeacherService teacherService)
        {
            _accountService = accountService;
            _teacherService = teacherService;
        }

        /// <summary>
        /// Dispay view model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creating new facultative for teacher
        /// </summary>
        /// <param name="model">view model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(FacultativeViewModel model)
        {
            if (Request.HttpMethod == "POST")
            {
                if (ModelState.IsValid)
                {
                    Teacher teacher = _teacherService.GetTeacher(_accountService.GetAccount(User.Identity.Name));

                    _teacherService.AddFacultative(new Facultative(model.Name, model.Theme, model.Description
                        , model.Status
                        , teacher
                        , model.StartFacultative
                        , model.Duration), teacher);

                    return RedirectToAction("Index", "TeacherFacultative");
                }
            }

            return View(model);
        }
    }
}