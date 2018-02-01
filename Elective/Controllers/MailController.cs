using BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elective
{ 
    [Authorize]
    public class MailController : Controller
    {
        private IAccountService _accountService;

        public MailController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Mail
        public ActionResult Index()
        {
            return View(_accountService.GetAllMessages(User.Identity.Name));
        }

        public ActionResult MailSend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(MailViewModel model)
        {
            if (ModelState.IsValid)
            {
                _accountService.SendMessage(User.Identity.Name, model.Login, model.Theme, model.Message);

                return RedirectToAction("Index", "Message", new { type = "Info", message = "Отправлено" });
            }

            return View(model);
        }

        public ActionResult MailReceive()
        {
            return View(_accountService.GetAllMessages(User.Identity.Name).MessagesReceive);
        }

        public ActionResult MailOptionR(Guid id)
        {
            return View("MailOption", _accountService.GetMessageReceive(User.Identity.Name, id));
        }

        public ActionResult MailOptionS(Guid id)
        {
            return View("MailOption",_accountService.GetMessageSend(User.Identity.Name, id));
        }

        public ActionResult MailSendOut()
        {
            return View(_accountService.GetAllMessages(User.Identity.Name).MessageSend);
        }

    }
}