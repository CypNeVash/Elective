using BusinessModel;
using System.Web.Mvc;

namespace Elective.Areas.Admin.Controllers
{
    /// <summary>
    /// Event log controller
    /// </summary>
    [ErrorExeptionFilter]
    [Authorize(Roles = Role.Admin)]
    public class LogsController : Controller
    {
        private IDefaultRepository<Log> _logRepository;

        public LogsController(IDefaultRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Display event Log
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(_logRepository.Get());
        }
    }
}