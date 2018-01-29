using BusinessModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controler for editing 
    /// facultative by teacher
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = "Teacher")]
    public class EditFacultativeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITeacherService _teacherService;

        public EditFacultativeController(IAccountService accountService, ITeacherService teacherService)
        {
            _accountService = accountService;
            _teacherService = teacherService;
        }

        /// <summary>
        /// View form
        /// </summary>
        public ActionResult Index(Guid id)
        {
            return View();
        }


        /// <summary>
        /// Creating facultative
        /// </summary>
        [HttpPost]
        public ActionResult Index(FacultativeViewModel facultativeVM, Guid id, string button)
        {
            var teacher = _teacherService.GetTeacher(_accountService.GetAccount(User.Identity.Name));
            var facultative = teacher.MyFacultatives.Where(s => s.Id == id).FirstOrDefault();
            facultative = _teacherService.GetFacultative(facultative);

            if (Request.HttpMethod == "POST")
            {

                if (button == "Change")
                {
                    foreach (var arg1 in facultative.GetType().GetProperties())
                    {
                        foreach (var arg2 in facultativeVM.GetType().GetProperties())
                        {
                            if (ModelState.IsValidField(arg1.Name) && arg1.Name == arg2.Name)
                            {
                                arg1.SetValue(facultative, arg2.GetValue(facultativeVM));
                            }
                        }
                    }
                    _teacherService.ChangeStateEntity(facultative);

                    return RedirectToAction("Index", "TeacherFacultative");
                }
                if (button == "Delete")
                {
                    _teacherService.DeleteFacultative(facultative);

                    return RedirectToAction("Index", "TeacherFacultative");
                }
            }

            return View(facultativeVM);
        }
    }
}