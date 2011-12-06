using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace CavemanTools.Mvc.Extensions
{
    
    public static class ControllerExtensions
    {
        /// <summary>
        /// Redirects to the selected action
        /// </summary>
        /// <typeparam name="T">Controller</typeparam>
        /// <param name="ctrl">controller</param>
        /// <param name="selector">lambda</param>
        /// <returns></returns>
        public static ActionResult RedirectToAction<T>(this T ctrl,Expression<Func<T,object>> selector) where T:Controller
         {
            return new RedirectToRouteResult(ToRouteValues(selector));            
         }

        /// <summary>
        /// Gets the invoked action for controller
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public static string GetActionName(this Controller ctrl)
        {
            if (ctrl == null) throw new ArgumentNullException("ctrl");
            return ctrl.RouteData.GetRequiredString("action");
        }

       internal static RouteValueDictionary ToRouteValues<T>(Expression<Func<T,object>> selector) where T:Controller
       {
           var method = selector.Body as MethodCallExpression;
           var action = method.Method.Name;
           var args = method.Method.GetParameters();
           var param = method.Arguments.Cast<ConstantExpression>().ToArray();

           var rv = new RouteValueDictionary();
           rv["action"] = action;
           var cname = typeof(T).Name;
           var cidx = cname.IndexOf("Controller");
           if (cidx > -1)
           {
               cname = cname.Remove(cidx, 10);
           }
           rv["controller"] = cname;
           foreach (var p in args)
           {
               rv[p.Name] = param[p.Position].Value;
           }
           return rv;
       }
    }
}