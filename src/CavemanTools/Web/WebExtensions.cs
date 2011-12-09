using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;

namespace CavemanTools.Web
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
		///  Returns subdomain from url
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
			for (int i = 0; i <= l; i++) d = d + all[i]+".";
			return d.Remove(d.Length-1,1);
		}

	   
        /// <summary>
        /// Tries to detect if the requested path is a static resource
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool MatchesStaticResource(this HttpRequest req)
        {
            return Regex.IsMatch(req.FilePath,@"(js|css|\.ico|\.jpg|\.gif)");
        }
	}
}