using BusinessModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// controler for editing users by admin
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = "Admin")]
    public class EditAccountController : Controller
    {
        private readonly IAccountService _accountService;

        public EditAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public ApplicationUserManager UserManager
        {
            get => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }
        /// <summary>
        /// Displaying form
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            return View();
        }


        /// <summary>
        /// Editing data
        /// </summary>
        /// <param name="accountVM">View model</param>
        /// <param name="button">Extra information</param>
        /// <param name="id">Id user</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(ChangeAccountDataViewModel accountVM, string button, Guid id)
        {
            if (Request.HttpMethod == "POST")
            {
                Account account = _accountService.GetAccount(User.Identity.Name);

                if (button == "Delete")
                {
                    User user = UserManager.FindById(id.ToString());
                    if (UserManager.IsInRole(user.Id, "Student"))
                        _accountService.DeleteStudent(user);
                    if (UserManager.IsInRole(user.Id, "Teacher"))
                        _accountService.DeleteTeacher(user);
                    UserManager.Delete(user);
                    return RedirectToAction("Index", "AccountManager");
                }
                else
                if (button == "Vac Ban")
                {
                    UserManager.SetLockoutEnabled(id.ToString(), true);
                    UserManager.SetLockoutEndDate(id.ToString(), DateTimeOffset.MaxValue);
                    return RedirectToAction("Index", "AccountManager");
                }
                else
                if (ModelState.IsValidField("Email"))
                {
                    UserManager.SetEmail(id.ToString(), accountVM.Email);
                }
                else
                if (ModelState.IsValidField("OldPassword") && ModelState.IsValidField("NewPassword"))
                {
                    var result = await UserManager.ChangePasswordAsync(id.ToString()
                        , accountVM.OldPassword
                        , accountVM.NewPassword);
                }
                else
                {
                    foreach (var arg1 in account.GetType().GetProperties())
                    {
                        foreach (var arg2 in accountVM.GetType().GetProperties())
                        {
                            if (ModelState.IsValidField(arg1.Name) && arg1.Name == arg2.Name)
                            {
                                arg1.SetValue(account, arg2.GetValue(accountVM));
                            }
                        }
                    }
                }    

                _accountService.ChangeStateEntity(account);

                return RedirectToAction("Index", "AccountManager");
            }

            return View(accountVM);
        }
    }
}