using BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elective.Controllers
{
    /// <summary>
    /// Controller for working 
    /// with personal data
    /// </summary>
    [Authorize]
    [ErrorExeptionFilter]
    public class PersonalAreaController : Controller
    {
        private IAccountService _accountService;

        public PersonalAreaController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        /// <summary>
        /// Displaying personal data
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return View(_accountService.GetAccount(User.Identity.Name));
        }

        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// Edit personal data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(PersonViewModel personVM)
        {
            if (Request.HttpMethod == "POST")
            {
                Account account = _accountService.GetAccount(User.Identity.Name);

                foreach (var arg1 in account.GetType().GetProperties())
                {
                    foreach (var arg2 in personVM.GetType().GetProperties())
                    {
                        if (ModelState.IsValidField(arg1.Name) && arg1.Name == arg2.Name)
                        {
                            arg1.SetValue(account, arg2.GetValue(personVM));
                        }
                    }
                }

                _accountService.ChangeStateEntity(account);
            }
            return View(personVM);
        }
    }
}