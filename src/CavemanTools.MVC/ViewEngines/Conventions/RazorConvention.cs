using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.ViewEngines.Conventions
{
    /// <summary>
    /// Default asp.net mvc views path searching with theming support
    /// </summary>
    public abstract class BaseRazorMvcConvention:IFindViewConvention
    {
        public bool Match(ControllerContext context, string viewName)
        {
            return true;
        }

        /// <summary>
        /// Gets relative path for view. 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public string GetViewPath(ControllerContext controllerContext, string viewName)
        {
            var controller = IsShared?"":controllerContext.RouteData.GetRequiredString("controller");
            var theme = controllerContext.HttpContext.GetCurrentTheme();
            var path = "~/Views/";
            if (!theme.IsNullOrEmpty())
            {
                path = "~/Themes/" + theme + "/Views/";                
            }

            return path+"{0}/{1}.cshtml".ToFormat(IsShared?"Shared":controller,viewName);
            
        }

        /// <summary>
        /// Serach in the Shared folder
        /// </summary>
        protected abstract bool IsShared { get; }
       
        /// <summary>
        /// Gets relative path for master.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public string GetMasterPath(ControllerContext controllerContext, string masterName)
        {
            if (masterName.IsNullOrEmpty()) return masterName;
            return "~/Views/Shared/{0}.cshtml".ToFormat(masterName);
        }
    }

    public class RazorControllerActionConvention:BaseRazorMvcConvention
    {
        protected override bool IsShared
        {
            get { return false; }
        }
    }

    public class RazorSharedFolderConvention:BaseRazorMvcConvention
    {
        /// <summary>
        /// Serach in the Shared folder
        /// </summary>
        protected override bool IsShared
        {
            get { return true; }
        }
    }
}