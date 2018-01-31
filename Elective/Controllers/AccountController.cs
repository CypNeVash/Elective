using BusinessModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Authorization and verification controllers
    /// </summary>
    [ErrorExeptionFilter]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        }

        public ApplicationUserManager UserManager
        {
            get => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = SignInManager.PasswordSignIn(model.Login, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                        new Log(GetType().ToString()
                        , "Login"
                        , LogStatus.info
                        , "SingIn"
                        , model.Login));
                    return RedirectToAction("Index", "PersonalArea");
                default:
                    DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                        new Log(GetType().ToString()
                        , "Login"
                        , LogStatus.error
                        , "SingIn"
                        , model.Login + " " + HttpContext.Request.UserHostAddress));
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Login,
                    Email = model.Email,
                };

                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Student");

                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                    DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                        new Log(GetType().ToString()
                        , "Register"
                        , LogStatus.info
                        , "Register"
                        , User.Identity.Name + " " + HttpContext.Request.UserHostAddress));

                    _accountService.CreateStudent(user,model.FirstName,model.SecondName,model.Age,model.BirthDate);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                        new Log(GetType().ToString()
                        , "LogOff"
                        , LogStatus.info
                        , "SingOut"
                        , User.Identity.Name + " " + HttpContext.Request.UserHostAddress));

            return RedirectToAction("Index", "Home");
        }
    }
}