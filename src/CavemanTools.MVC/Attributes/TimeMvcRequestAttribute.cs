using System;
using System.Web.Mvc;
using CavemanTools.Web;

namespace CavemanTools.Mvc.Attributes
{
    /// <summary>
    /// RequestStatsModule is required to be active, in order to show the metrics
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeMvcRequestAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcActionDuration);            
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcActionDuration);            
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcResultDuration);            
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcResultDuration);            
        }
    }
}