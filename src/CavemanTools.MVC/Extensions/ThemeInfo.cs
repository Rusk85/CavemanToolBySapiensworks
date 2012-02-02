using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CavemanTools.Mvc.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ThemeInfo
    {
        internal ThemeInfo(HttpContextBase ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");
            Name = ctx.GetCurrentTheme();
            BaseUrl=UrlHelper.GenerateContentUrl(string.Format("~/Themes/{0}", Name),ctx);
            StyleUrl = BaseUrl + "/Style";
            ScriptsUrl = BaseUrl + "/Scripts";
            ViewsPath = string.Format("~/Themes/{0}/Views", Name);
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

        public string ViewsPath { get; private set; }
    }
}