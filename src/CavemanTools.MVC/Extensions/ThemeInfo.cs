using System;
using System.Web;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ThemeInfo
    {
        public static string ThemesDirectoryName = "Themes";
        public static string CssDirectoryName = "Style";
        public static string ScriptsDirectoryName = "Scripts";

        internal ThemeInfo(HttpContextBase ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            Name = ctx.GetCurrentTheme();
            BaseUrl=UrlHelper.GenerateContentUrl("~/"+ThemesDirectoryName+"/"+Name,ctx);
            StyleUrl = BaseUrl + "/"+CssDirectoryName;
            ScriptsUrl = BaseUrl + "/"+ScriptsDirectoryName;
            ViewsPath = string.Format("~/{1}/{0}/Views", Name,ThemesDirectoryName);
        }
        /// <summary>
        /// Gets current theme name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the client url for the theme style directory
        /// </summary>
        public string StyleUrl { get; private set; }
        /// <summary>
        /// Gets the client url for the theme scripts directory
        /// </summary>
        public string ScriptsUrl { get; private set; }
        /// <summary>
        /// Gets the client url for the theme 
        /// </summary>
        public string BaseUrl { get; private set; }
        /// <summary>
        /// Relative path of the views directory. Ex: ~/themes/default/views
        /// </summary>
        public string ViewsPath { get; private set; }
    }
}