using System;
using System.Linq;
using System.Web;

namespace CavemanTools.Mvc.Extensions
{
	public static class Common
	{
		
		/// <summary>
		/// Gets the IP of the user  detects proxy
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string RealIp(this HttpRequestBase request)
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
	}
	
}