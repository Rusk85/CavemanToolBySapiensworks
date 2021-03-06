using System.Text.RegularExpressions;
using System.Linq;

namespace System.Web
{
	public static class WebExtensions
	{
		/// <summary>
		/// Gets the IP of the user  detects proxy
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string RealIp(this HttpRequest request)
		{
			if (request == null) throw new ArgumentNullException("request");
			var fip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if ( fip!= null)
			{
				var ip = fip.Split(',').LastOrDefault();
				if (!string.IsNullOrEmpty(ip)) return ip.Trim();				
			}
			return request.UserHostAddress;
		}

		/// <summary>
		///  Returns subdomain from url. It covers only some common cases.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <param name="host">Host name</param>
		/// <returns></returns>
		public static string ExtractSubdomain(this string  host)
		{
			if (string.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            var all = host.Split('.');
            if (all.Length < 3) return string.Empty;
            var l = all.Length - 3;
            string d = string.Empty;
            for (int i = 0; i <= l; i++) d = d + all[i] + ".";
            return d.Remove(d.Length - 1, 1);
		}

       //public static string GetSubdomain(this Uri url)
       //{
       //    if (url.HostNameType == UriHostNameType.Dns)
       //    {
       //        return url.Host.ExtractSubdomain();
       //    }
       //    return null;
       //}

        /// <summary>
        /// Tries to detect if the requested path is a static resource
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool MatchesStaticResource(this HttpRequest req)
        {
            return Regex.IsMatch(req.FilePath,@"(js|css|ico|jpe?g|gif|png|bmp|html?)$");
        }

        /// <summary>
        /// Gets an object from context items 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpContextBase ctx, string key, T defaultValue = default(T))
        {
            ctx.MustNotBeNull();
            key.MustNotBeEmpty();
            var rez = defaultValue;
            if (ctx.Items.Contains(key))
            {
                rez = (T)ctx.Items[key];
            }
            return rez;
        }

        public static void Set<T>(this HttpContextBase ctx, string key, T value)
        {
            ctx.MustNotBeNull();
            key.MustNotBeEmpty();
            ctx.Items[key] = value;
        }

        /// <summary>
        /// Gets an object from context items 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this HttpContext ctx, string key, T defaultValue = default(T))
        {
            var rez = defaultValue;
            if (ctx.Items.Contains(key))
            {
                rez = (T)ctx.Items[key];
            }
            return rez;
        }

        public static string HtmlEncode(this string data)
        {
            data.MustNotBeEmpty();
            return HttpUtility.HtmlEncode(data);
        }

        public static string HtmlDecode(this string data)
        {
            return HttpUtility.HtmlDecode(data);
        }

        public static string UrlEncode(this string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        public static string UrlDecode(this string data)
        {
            return HttpUtility.UrlDecode(data);
        }

	}
}