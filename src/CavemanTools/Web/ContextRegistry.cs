using System.Web;

namespace CavemanTools.Web
{
    public static class ContextRegistry
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