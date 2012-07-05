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

       public static void Set(this HttpContext ctx,string key, object value)
       {
           ctx.Items[key] = value;
       }

        public static T Get<T>(string key,T defaultValue=default(T))
        {
            return HttpContext.Current.Get(key, defaultValue);
        }
     
    }
}