using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Controllers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ModelIsArgumentAttribute:ActionFilterAttribute
    {
        /// <summary>
        /// Sets the name of the action argument which is the model
        /// </summary>
        /// <param name="paramName"></param>
        public ModelIsArgumentAttribute(string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException("paramName");
            ParameterName = paramName;
        }

        /// <summary>
        /// Sets the position of the model in action's argument list
        /// </summary>
        /// <param name="position"></param>
        public ModelIsArgumentAttribute(int position)
        {
            Position = position;
        }

        public int Position { get; private set; }

        public string ParameterName { get; private set; }
    }
}