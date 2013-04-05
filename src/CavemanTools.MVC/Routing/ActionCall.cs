using System;
using System.Linq;
using System.Reflection;
using System.Web.Routing;

namespace CavemanTools.Mvc.Routing
{
    public class ActionCall
    {
        private Type _controller;
        private MethodInfo _method;

        public ActionCall(MethodInfo method,RoutingPolicySettings handler)
        {
            method.MustNotBeNull();
            Settings = handler;
            _controller = method.DeclaringType;
            _method = method;
        }

        public Type Controller
        {
            get { return _controller; }
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        public RoutingPolicySettings Settings { get; private set; }
        /// <summary>
        /// Creates a route value dictionary with controller and action values set
        /// </summary>
        /// <returns></returns>
        public RouteValueDictionary CreateDefaults()
        {
            var defaults = new RouteValueDictionary();
            var controler = Controller.Name;
            if (controler.EndsWith("Controller"))
            {
                controler = controler.Substring(0, controler.Length - 10);
            }
            defaults["controller"] = controler;
            defaults["action"] = Method.Name;
            return defaults;
        }

        /// <summary>
        /// Sets the defaults for the route params. Only action parameters with default values are considered.
        /// User defined params are ignored.
        /// This method should not be used for POST.
        /// </summary>
        /// <param name="defaults"></param>
        public void SetParamsDefaults(RouteValueDictionary defaults)
        {
            var param = Method.GetParameters().Where(p => p.RawDefaultValue != DBNull.Value && !TypeExtensions.IsUserDefinedClass(p.ParameterType));
            foreach (var p in param)
            {
                defaults[p.Name] = p.RawDefaultValue;
            }
        }
    }
}