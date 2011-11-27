using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Extensions
{
	public static class ThemesExtensions
	{
		/// <summary>
		/// Returns the path for a static theme resource (css,js, file).
		/// The resource should be located into ~/content/[theme_name]/ directory
		/// </summary>
		/// <param name="url"></param>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string ThemeResource(this UrlHelper url, string file)
		{
			if (file == null) throw new ArgumentNullException("file");
			if (url == null) throw new ArgumentNullException("url");
			return url.Content(string.Format("~/content/{0}/{1}", url.GetCurrentTheme(), file));
		}

		public static string GetCurrentTheme(this UrlHelper url)
		{
			if (url == null) throw new ArgumentNullException("url");
			return (string)url.RequestContext.HttpContext.Items["theme"];
		}

	}
}