using System.Linq.Expressions;
using System.Web.Routing;
using System.Linq;
using CavemanTools.Model;
using CavemanTools.Mvc;
using CavemanTools.Mvc.Extensions;


namespace System.Web.Mvc
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Redirects to the selected action
        /// </summary>
        /// <typeparam name="T">Controller</typeparam>
        /// <param name="ctrl">controller</param>
        /// <param name="selector">lambda statement</param>
        /// <returns></returns>
        public static ActionResult RedirectToAction<T>(this T ctrl,Expression<Action<T>> selector) where T:Controller
         {
            return new RedirectToRouteResult(ToRouteValues(selector));            
         }

        /// <summary>
        /// Redirects to the selected action from another controller
        /// </summary>
        /// <typeparam name="T">Controller class</typeparam>
        /// <param name="c"></param>
        /// <param name="selector">lambda statement</param>
        /// <returns></returns>
        public static ActionResult RedirectToController<T>(this Controller c,Expression<Action<T>> selector) where T:Controller
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

        /// <summary>
        /// Handle the specified command using the current DependecyResolver
        ///  to resolve the command handler. 
        /// Use <see cref="BusinessRuleException"/> in the command handler 
        /// to send the errors to the ModelState
        /// </summary>
        /// <typeparam name="T">Reference type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command object</param>
        public static void Handle<T>(this Controller ctrl,T cmd) where T:class
        {
            ctrl.Handle(cmd,DependencyResolver.Current);
        }

        /// <summary>
        /// Handle the specified command using the specified command handler
        /// Use <see cref="BusinessRuleException"/> in the command handler 
        /// to send the errors to the ModelState
        /// </summary>
        /// <typeparam name="T">Reference type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command object</param>
        /// <param name="handler">Handler implementation </param>
        public static void Handle<T>(this Controller ctrl,T cmd,IHandleCommand<T> handler) where T:class
        {
            ctrl.Handle(cmd,handler.Handle);
        }

        /// <summary>
        /// Handle the specified command using the specified command handler
        /// Use <see cref="BusinessRuleException"/> in the command handler 
        /// to send the errors to the ModelState
        /// </summary>
        /// <typeparam name="T">Reference type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command object</param>
        /// <param name="handler">Handler implementation </param>
        public static void Handle<T>(this Controller ctrl, T cmd, Action<T> handler) where T : class
        {
            if (ctrl == null) throw new ArgumentNullException("ctrl");
            if (cmd == null) throw new ArgumentNullException("cmd");
            if (handler == null) throw new ArgumentNullException("handler");

            try
            {
                handler(cmd);
            }
            catch (BusinessRuleException ex)
            {
                ctrl.ModelState.AddModelError(ex.Name,ex.Message);                
            }
        }


        /// <summary>
        /// Processes a command and returns a result.
        /// Request handlers must be registered with a DI Container
        /// Useful if you want to return data required for the controller to continue if command was successful.
        /// </summary>
        /// <typeparam name="T">Command Type</typeparam>
        /// <typeparam name="R">Result type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command</param>
        /// <returns></returns>
        public static R Process<T,R>(this Controller ctrl,T cmd) where T:class 
        {
            return ctrl.Process<T,R>(cmd, DependencyResolver.Current);
        }

        /// <summary>
        /// Processes a command and returns a result.
        /// Useful if you want to return data required for the controller to continue if command was successful.
        /// </summary>
        /// <typeparam name="T">Command Type</typeparam>
        /// <typeparam name="R">Result type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command</param>
        /// <param name="ioc">Container to resolve handler </param>
        /// <returns></returns>
        public static R Process<T,R>(this Controller ctrl,T cmd,IDependencyResolver ioc) where T:class 
        {
            if (ioc == null) throw new ArgumentNullException("ioc");
            var handler = ioc.GetService<IHandleRequest<T,R>>();
            return Process(ctrl, cmd, handler);
        }

        /// <summary>
        /// Processes a command and returns a result.
        /// Useful if you want to return data required for the controller to continue if command was successful.
        /// </summary>
        /// <typeparam name="T">Command Type</typeparam>
        /// <typeparam name="R">Result type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command</param>
        /// <param name="handler">Handler implementation</param>
        public static R Process<T,R>(this Controller ctrl,T cmd,IHandleRequest<T,R> handler) where T:class 
        {
            return ctrl.Process(cmd, handler.Process);
        }

        /// <summary>
        /// Processes a command and returns a result.
        /// Useful if you want to return data required for the controller to continue if command was successful.
        /// </summary>
        /// <typeparam name="T">Command Type</typeparam>
        /// <typeparam name="R">Result type</typeparam>
        /// <param name="ctrl">Controller</param>
        /// <param name="cmd">Command</param>
        /// <param name="handler">Handler implementation</param>
        public static R Process<T,R>(this Controller ctrl,T cmd,Func<T,R> handler)
        {
            if (ctrl == null) throw new ArgumentNullException("ctrl");
            if (cmd == null) throw new ArgumentNullException("cmd");
            if (handler == null) throw new ArgumentNullException("handler");

            try
            {
                return handler(cmd);
            }
            catch (BusinessRuleException ex)
            {
                ctrl.ModelState.AddModelError(ex.Name, ex.Message);
            }
            return default(R);
        }

        /// <summary>
        /// Handle the specified command using the specified dependency resolver
        ///  to resolve the command handler
        /// </summary>
        /// <typeparam name="T">Reference type</typeparam>
        /// 
        /// <param name="ctrl">Controller</param>
        /// <param name="ioc">Implementation of IoC container</param>
        /// <param name="cmd">Command object</param>
       public static void Handle<T>(this Controller ctrl,T cmd,IDependencyResolver ioc) where T:class 
       {
           if (ioc == null) throw new ArgumentNullException("ioc");
           var handler = ioc.GetService<IHandleCommand<T>>();
           Handle(ctrl,cmd,handler);
       }

       internal static RouteValueDictionary ToRouteValues<T>(Expression<Action<T>> selector) where T:Controller
       {
           var method = selector.Body as MethodCallExpression;
           var action = method.Method.Name;
           var args = method.Method.GetParameters();
            var param = method.Arguments.ToArray();
           
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
               rv[p.Name] =ExpressionParamValue.ForExpression(param[p.Position]);
           }
           return rv;
       }
    }
}