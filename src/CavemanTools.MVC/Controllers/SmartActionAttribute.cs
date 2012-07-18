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
        /// <summary>
        /// True to disable smart controller for this action only
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Use this view to display the invalid model
        /// </summary>
        public string ViewName { get; set; }
        public SmartActionAttribute()
        {
            Order = 100;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctrl = (SmartController) filterContext.Controller;
            if (ctrl==null) throw new NotSupportedException("This attribute works only with SmartController");
            if (!Ignore)ctrl.HandleActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ctrl = (SmartController)filterContext.Controller;
            if (ctrl == null) throw new NotSupportedException("This attribute works only with SmartController");
            if (!Ignore)ctrl.HandleActionExecuted(filterContext);
        }
    }
}