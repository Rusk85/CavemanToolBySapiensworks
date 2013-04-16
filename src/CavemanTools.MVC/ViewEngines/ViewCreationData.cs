using System.Web.Mvc;

namespace CavemanTools.Mvc.ViewEngines
{
    public class ViewCreationData
    {
        public ControllerContext Context { get; private set; }
        public string ViewPath { get; private set; }
        /// <summary>
        /// Should be ignored for partials
        /// </summary>
        public string MasterPath { get; private set; }
        /// <summary>
        /// True if the view is a mvc template i.e partials from DisplayTemplates or EditorTemplates
        /// </summary>
        public bool IsMvcTemplate { get; set; }

        internal ViewCreationData(ControllerContext context,string viewPath,string masterPath)
        {
            Context = context;
            ViewPath = viewPath;
            MasterPath = masterPath;
        }
    }
}