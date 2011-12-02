using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CavemanTools.Mvc.Extensions;

namespace CavemanTools.Mvc.Controllers
{
    /// <summary>
    /// Base controller that handles automatically invalid model state.
    /// </summary>
    public abstract class SmartController:BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            if (HttpContext.Request.IsPost())
            {
                currentModel = null;
               EstablishModel(filterContext.ActionParameters,filterContext.ActionDescriptor);
               
                if (!ModelState.IsValid)
                {
                    filterContext.Result = ActionFailResult(currentModel)();
                }
            }
        }

        void EstablishModel(IDictionary<string,object> args,ActionDescriptor ad)
        {
            if (args.Count >1)
            {

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
                currentModel = args.Values.FirstOrDefault();
            }
            if(currentModel!=null && !(currentModel.GetType().IsClass))
            {
                throw new InvalidModelException("Model must be of a reference type"); 
            }
        }

        private dynamic currentModel;

    
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
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