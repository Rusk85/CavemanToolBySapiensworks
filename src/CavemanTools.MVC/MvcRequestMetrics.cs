﻿using System.Web.Mvc;
using CavemanTools.Web;

namespace CavemanTools.Mvc
{
    /// <summary>
    /// RequestStatsModule is required to be active, in order to show the metrics
    /// </summary>    
    public class MvcRequestMetrics:IActionFilter,IResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RequestMetricsModule.HandleDuration(RequestMetricsModule.MvcActionDuration);            
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            RequestMetricsModule.HandleDuration(RequestMetricsModule.MvcActionDuration);            
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            RequestMetricsModule.HandleDuration(RequestMetricsModule.MvcResultDuration);            
        }
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RequestMetricsModule.HandleDuration(RequestMetricsModule.MvcResultDuration);            
        }
    }
}