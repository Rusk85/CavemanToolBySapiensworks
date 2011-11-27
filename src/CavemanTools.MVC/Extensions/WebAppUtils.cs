using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CavemanTools.Extensions;

namespace CavemanTools.Mvc.Extensions
{
	public static class WebAppUtils
	{
		/// <summary>
		/// Render error view 
		/// </summary>
		/// <param name="ctx">HttpContext</param>
		/// <param name="view">View name</param>
		/// <param name="viewData">For ViewData</param>
		public static void HandleError(HttpContext ctx,string view,object viewData)
		{
			if (ctx == null) throw new ArgumentNullException("ctx");
			if (view == null) throw new ArgumentNullException("view");
			ctx.Response.Clear();
			
			RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
			string controllerName = rc.RouteData.GetRequiredString("controller");
			IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
			IController controller = factory.CreateController(rc, controllerName);
			ControllerContext cc = new ControllerContext(rc, (ControllerBase)controller);
			ViewResult viewResult = new ViewResult { ViewName = view };
			if (viewData != null)
			{
				foreach (var kv in viewData.ToDictionary())
				{
					viewResult.ViewData.Add(kv.Key, kv.Value);
				}
			}
			viewResult.ExecuteResult(cc);
			ctx.Server.ClearError(); 
		}
	}
}