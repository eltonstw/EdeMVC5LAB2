using System;
using System.Web.Mvc;

namespace LAB.Controllers
{
    public class ExecTimeLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.StartDt = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.ExecutingHowLong =
                (DateTime.Now - (DateTime) filterContext.Controller.ViewBag.StartDt).TotalMilliseconds;

            base.OnActionExecuted(filterContext);
        }
    }
}