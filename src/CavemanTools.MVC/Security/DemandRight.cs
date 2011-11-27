using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Security
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class DemandRightAttribute:AuthorizeAttribute
    {
       
        public string Right { get; set; }

      public override void OnAuthorization(AuthorizationContext filterContext)
        {
           if (!AuthorizeCore(filterContext.HttpContext) || Right==null) return;
           var dt = filterContext.HttpContext.GetUserContext();
           if (dt == null)
           {
               HandleUnauthorizedRequest(filterContext);
           }
           else
           {
               if (!dt.HasRight(Right))
               {
                   filterContext.Result = new HttpStatusCodeResult(403, "You have no rights for this");
               }
           }
            
        }
    }
}