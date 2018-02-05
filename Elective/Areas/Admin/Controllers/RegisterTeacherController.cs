using BusinessModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Controller for adding teacher
    /// by admin
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Admin)]
    public class RegisterTeacherController : Controller
    {
        private readonly IAccountService _accountService;

        public RegisterTeacherController(IAccountService accountService)
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
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creating teacher
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Login,
                    Email = model.Email,
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _accountService.CreateTeacher(user,model.FirstName,model.SecondName,model.Age,model.BirthDate);
                    UserManager.AddToRole(user.Id, Role.Teacher);

                    return RedirectToAction("Index", "AccountManager");
                }
            }

            return View(model);
        }
    }
}