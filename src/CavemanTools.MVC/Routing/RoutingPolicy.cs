using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using System.Reflection;

namespace CavemanTools.Mvc.Routing
{
    public class RoutingPolicy
    {
        public RoutingPolicy()
        {
            UrlFormatPolicies=new List<IRouteUrlFormatPolicy>();    
            Conventions=new List<IRouteConvention>();
            Settings=new RoutingPolicySettings();
            Settings.NamespaceRoot = Assembly.GetCallingAssembly().GetName().Name + ".Controllers";
            GlobalPolicies= new List<IGlobalRoutePolicy>();
        }

        public RoutingPolicySettings Settings { get; private set; }
        List<ActionCall> _actions=new List<ActionCall>();
        public void AddAction(ActionCall action)
        {
            action.MustNotBeNull();
            _actions.Add(action);
            
            if (action.Method.HasCustomAttribute<HomepageAttribute>())
            {
                _homepage = action;
            }
        }

        private ActionCall _homepage;

        /// <summary>
        /// Gets a list of url policies (url format) 
        /// which will be applied for any matching action
        /// </summary>
        public IList<IRouteUrlFormatPolicy> UrlFormatPolicies { get; private set; }
        /// <summary>
        /// Gets a list of conventions used to create routes
        /// </summary>
        public IList<IRouteConvention> Conventions { get; private set; }

        /// <summary>
        /// Gets a list of policies that apply to every route
        /// </summary>
        public IList<IGlobalRoutePolicy> GlobalPolicies { get; private set; }

        
        public void Apply(RouteCollection routeCollection)
        {
            foreach (var action in _actions)
            {
                foreach (var convention in Conventions.Where(c => c.Match(action)))
                {
                    var routes = convention.Build(action);
                    foreach (var formatter in UrlFormatPolicies.Where(u => u.Match(action)))
                    {
                        foreach (var route in routes)
                        {
                            route.Url = formatter.Format(route.Url, action);
                        }
                    }

                    GlobalPolicies.ForEach(p=>routes.ForEach(r=>p.ApplyTo(r)));

                    routes.ForEach(r=>routeCollection.Add(r));                    
                }
            }
            HandleHomepage(routeCollection);
        }

        void HandleHomepage(RouteCollection routes)
        {
            if (_homepage == null) return;
            var defaults = _homepage.CreateDefaults();
            _homepage.SetParamsDefaults(defaults);
            var home = _homepage.Method.GetSingleAttribute<HomepageAttribute>();
            var route = new Route(home.Url, defaults, new RouteValueDictionary(), _homepage.Settings.CreateHandler());
            GlobalPolicies.ForEach(p => p.ApplyTo(route));
            routes.Add(route);
        }
    }
}