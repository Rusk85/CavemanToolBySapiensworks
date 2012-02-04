
using System.Text;

namespace System.Web
{
	public static  class CookieExtensions
	{
		/// <summary>
		/// Updates or appends a cookie to collection
		/// </summary>
		/// <param name="cookies"></param>
		/// <param name="cookie"></param>
        public static void Attach(this HttpCookieCollection cookies,HttpCookie cookie)
		{
			if (cookies == null) throw new ArgumentNullException("cookies");
			if (cookie == null) throw new ArgumentNullException("cookie");
			if (cookies[cookie.Name]==null)
			{
				cookies.Add(cookie);
			}
			else
			{
				cookies.Set(cookie);
			}
		}
		
		/// <summary>
		/// Gets typed object from cookie value or a specified default value if invalid cookie
		/// </summary>
		/// <typeparam name="T">object type</typeparam>
		/// <param name="cookie"></param>
		/// <param name="defaultValue">Value to return if cookie doesn't contain a valid value</param>
		/// <returns></returns>
		public static T GetValue<T>(this HttpCookie cookie, T defaultValue)
		{
			if (cookie == null) throw new ArgumentNullException("cookie");
			return cookie.Value.Parse(defaultValue);
		}

		/// <summary>
		/// Gets typed object from cookie value, or default for the type if invalid cookie value
		/// </summary>
		/// <typeparam name="T">object type</typeparam>
		/// <param name="cookie"></param>
		/// <returns></returns>
		public static T GetValue<T>(this HttpCookie cookie)
		{
			return GetValue(cookie, default(T));
		}

		/// <summary>
		/// Gets typed object from cookie value using parser 
		/// </summary>
		/// <typeparam name="T">object type</typeparam>
		/// <param name="cookie"></param>
		/// <param name="parser">Parser to create object from string</param>
		/// <returns></returns>
		public static T GetValue<T>(this HttpCookie cookie, IStringParser<T> parser)
		{
			if (cookie == null) throw new ArgumentNullException("cookie");
			if (parser == null) throw new ArgumentNullException("parser");
			return cookie.Value.Parse(parser);
		}
	}
}