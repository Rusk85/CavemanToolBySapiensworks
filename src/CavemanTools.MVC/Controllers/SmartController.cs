using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace CavemanTools.Mvc.Controllers
{
    /// <summary>
    /// Base controller that handles automatically invalid model state.
    /// </summary>
    public abstract class SmartController:BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attr = filterContext.ActionDescriptor.GetSingleAttribute<SmartActionAttribute>();
            if (attr==null)
            {
                HandleActionExecuting(filterContext);
            }
        }

        internal void HandleActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.IsPost())
            {
                currentModel = null;
                EstablishModel(filterContext.ActionParameters, filterContext.ActionDescriptor);
                if (!ModelState.IsValid)
                {
                    filterContext.Result = ActionFailResult(currentModel)();
                }
            }
        }

        protected new ActionResult UpdateModel<T>(T model, Action<T> action, Func<ActionResult> success = null, Func<ActionResult> failure = null)
        {
            throw new NotSupportedException("UpdateModel is not supported by SmartController");
        }

        void EstablishModel(IDictionary<string,object> args,ActionDescriptor ad)
        {
            if (args.Count >1)
            {
                //check if model param is specified by attribute
                var attr = ad.GetCustomAttributes(typeof (ModelIsArgumentAttribute), true).Cast<ModelIsArgumentAttribute>().FirstOrDefault();
                if (attr!=null)
                {
                    if (attr.ParameterName!=null)
                    {
                        currentModel =args.Where(p => p.Key == attr.ParameterName).Select(d => d.Value).FirstOrDefault();
                    }
                    else
                    {
                        currentModel = args.Values.Skip(attr.Position).Take(1).FirstOrDefault();
                    }
                }
            }
            
            if(currentModel==null)
            {
                //we setup the first param as the model
                currentModel = args.Values.FirstOrDefault();
            }
            
            //if model is a value type ignore
            if(currentModel!=null && !(currentModel.GetType().IsClass))
            {
               Debug.WriteLine("Action argument which is not an object is ignored by SmartController");
                // throw new InvalidModelException("Model must be of a reference type"); 
            }
        }

        private dynamic currentModel;

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var attr = filterContext.ActionDescriptor.GetSingleAttribute<SmartActionAttribute>();
            if (attr == null)
            {
                HandleActionExecuted(filterContext);
            }
        }

        internal void HandleActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Canceled) return;
            if (filterContext.HttpContext.Request.IsPost())
            {
                if (!ModelState.IsValid)
                {
                    filterContext.Result = ActionFailResult(currentModel)();
                }
            }
        }
    }
}