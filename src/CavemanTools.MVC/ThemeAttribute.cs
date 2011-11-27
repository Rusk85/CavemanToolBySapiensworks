using System;
using System.Web.Mvc;
using CavemanTools.Web;

namespace CavemanTools.Mvc
{
	public class ThemeAttribute:ActionFilterAttribute
	{
		/// <summary>
		/// Gets or sets the default theme
		/// </summary>
		public string Default { get; set; }

		/// <summary>
		/// Query parameter name for theme
		/// Default is 'theme'
		/// </summary>
		public string ParamName { get; set; }

		public ThemeAttribute()
		{
			Default = "default";
			ParamName = "theme";
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext == null) throw new ArgumentNullException("filterContext");
			var ctx = filterContext.HttpContext;
			var th = new RequestPersonalizationParameter<string>(ParamName);
			
			if (th.LoadFromString(ctx.Request.QueryString[ParamName]))
			{
				th.Cache();
			}
			else
			{
				th.LoadFromCache();
			}
			ctx.Items["theme"] = th.Value ?? Default ?? "default";
		}
	}
}