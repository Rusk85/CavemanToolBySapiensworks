using System;
using System.Web.Mvc;
using CavemanTools.Web;

namespace CavemanTools.Mvc.Attributes
{
    /// <summary>
    /// RequestStatsModule is required to be active, in order to show the metrics
    /// </summary>    
    public class MvcRequestMetrics:IActionFilter,IResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcActionDuration);            
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcActionDuration);            
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcResultDuration);            
        }
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RequestStatsModule.HandleDuration(RequestStatsModule.MvcResultDuration);            
        }
    }
}