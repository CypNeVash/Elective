using BusinessModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for creating statement
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Teacher)]
    public class AddLogController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IAccountService _accountService;

        public AddLogController(ITeacherService teacherService, IAccountService accountService)
        {
            _teacherService = teacherService;
            _accountService = accountService;
        }


        /// <summary>
        /// Creating report book
        /// </summary>
        /// <param name="id">id facultative</param>
        /// <param name="reportBookVM">view model reports</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(Guid id, ReportBookViewModel reportBookVM)
        {
            Teacher teacher = _teacherService.GetTeacher(_accountService.GetAccount(User.Identity.Name));

            Facultative facultative = teacher.MyFacultatives.Where(s => s.Id == id).FirstOrDefault();

            if (facultative == null)
                RedirectToAction("Index", "Message", new { type = "Error", Message = "Dont find facultative" });

            facultative = _teacherService.GetFacultative(new Facultative() { Id = id });

            reportBookVM.Elective = facultative;

            ReportBook reportBook = _teacherService.CreateReportBook(facultative.Name, facultative);

            if (Request.HttpMethod == "POST")
            {
                if (ModelState.IsValid)
                {
                    if (reportBook.Reports.Count > 0)
                        reportBook.Reports.Clear();

                    foreach (var item in reportBookVM.Reports)
                    {
                        facultative.Status = FacultativeStatus.Finished;

                        _teacherService.AddReport(reportBook, new Report(facultative.Audience.Where(s => s.Id == item.Id).FirstOrDefault(), item.Mark));
                    }

                    _teacherService.ChangeStateEntity(facultative);
                    _teacherService.ChangeStateEntity(reportBook);

                    return RedirectToAction("Index", "TeacherFacultative", new { id = id });
                }
            }

            _teacherService.ChangeStateEntity(reportBook);

            return View(reportBookVM);
        }

    }
}