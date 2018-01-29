using System;
using System.Web.Mvc;
using BusinessModel;

namespace Elective
{
    [AttributeUsage(AttributeTargets.All)]
    public class ErrorExeptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var Url = new UrlHelper(filterContext.RequestContext);

            var url = Url.Action("Index", "Message",new { message = "Something went wrong", type = "Error" });

            DependencyResolver.Current.GetService<IDefaultRepository<Log>>().Add(
                new Log(filterContext.RouteData.Values["controller"] as String
                , filterContext.RouteData.Values["action"] as String
                , LogStatus.error
                , filterContext.Exception.Message
                , filterContext.HttpContext.User.Identity.Name + " " + filterContext.HttpContext.Request.UserHostAddress));


            filterContext.Result = new RedirectResult(url);

            filterContext.ExceptionHandled = true;

        }
    }
}