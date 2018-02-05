using BusinessModel;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Elective
{
    /// <summary>
    /// Mail controller
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize]
    public class MailController : Controller
    {
        private IAccountService _accountService;

        public MailController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Page for send message "GET" http method
        /// </summary>
        /// <returns></returns>
        public ActionResult MailSend()
        {
            return View();
        }

        /// <summary>
        /// Send message action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return all receive messages for user 
        /// </summary>
        /// <returns></returns>
        public ActionResult MailReceive()
        {
            var messages = _accountService.GetAllMessages(User.Identity.Name).MessagesReceive;

            HttpContext.Cache.Insert(User.Identity.Name.ToLower(), messages.Where(s => s.Status == MessageState.NotRead).Count());

            return View(messages.OrderByDescending(s => s.SendDate).OfType<Message>().ToList());
        }

        /// <summary>
        /// Return select receieve message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MailOptionR(Guid id)
        {
            bool notRead = false;

            Message message = _accountService.GetMessageReceive(User.Identity.Name, id, ref notRead);

            if (notRead)
                HttpContext.Cache.Insert(User.Identity.Name, (int)HttpContext.Cache.Get(User.Identity.Name) - 1);

            return View("MailOption", message);
        }

        /// <summary>
        /// Return select send message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MailOptionS(Guid id)
        {
            return View("MailOption", _accountService.GetMessageSend(User.Identity.Name, id));
        }

        /// <summary>
        /// Return all user send out messages
        /// </summary>
        /// <returns></returns>
        public ActionResult MailSendOut()
        {
            return View(_accountService.GetAllMessages(User.Identity.Name)
                .MessageSend.OrderByDescending(s => s.SendDate).OfType<Message>().ToList());
        }

    }
}