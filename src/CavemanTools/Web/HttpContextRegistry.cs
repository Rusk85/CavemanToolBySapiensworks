using System.Web;

namespace CavemanTools.Web
{
    /// <summary>
    /// Helper to set/get objects in the current http context
    /// </summary>
    public static class HttpContextRegistry
    {
       public static void Set(string key,object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        public static T Get<T>(string key,T defaultValue)
        {
            return HttpContext.Current.Get(key, defaultValue);
        }
    }
}