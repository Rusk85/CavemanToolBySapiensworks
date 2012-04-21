using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Controllers
{
    /// <summary>
    /// Use this attribute to trigger the SmartController automatic model validation handling
    /// AFTER other action filters.
    /// By default, SmartController handles the model validation BEFORE any other controller/action filter
    /// and if the model state is invalid, the other filters are NOT triggered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SmartActionAttribute:ActionFilterAttribute
    {
        public SmartActionAttribute()
        {
            Order = 100;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctrl = (SmartController) filterContext.Controller;
            if (ctrl==null) throw new NotSupportedException("This attribute works only with SmartController");
            ctrl.HandleActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ctrl = (SmartController)filterContext.Controller;
            if (ctrl == null) throw new NotSupportedException("This attribute works only with SmartController");
            ctrl.HandleActionExecuted(filterContext);
        }
    }
}