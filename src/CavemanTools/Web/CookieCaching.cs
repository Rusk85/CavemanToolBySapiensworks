using System;
using System.Web;
using CavemanTools.Strings;

namespace CavemanTools.Web
{

	/// <summary>
	/// Caches a single object in a cookie
	/// </summary>
	/// <typeparam name="T">Type of object</typeparam>
	public class CookieCaching<T>:ICookieCache<T>
	{

		public CookieCaching()
		{

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="name">Cookie name</param>
		/// <param name="value">Value</param>
		public CookieCaching(string name, T value)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
			CookieName = name;
			Value = value;
		}

		/// <summary>
		/// Gets or sets object 
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Gets or sets cookie name which stores the value of parameter
		/// </summary>
		public string CookieName { get; set; }


		/// <summary>
		/// Saves value into response cookie.
		/// Uses object's ToString()
		/// </summary>
		/// <param name="response">Response cookie collection</param>
		public void Store(HttpCookieCollection response)
		{
			if (response == null) throw new ArgumentNullException("response");
			if (string.IsNullOrEmpty(CookieName)) throw new InvalidOperationException("CookieName is empty");
			Cookie = new HttpCookie(CookieName, Value.ToString());

			var ctx = HttpContext.Current;
			if (ctx != null)
			{
				Cookie.Path = ctx.Request.ApplicationPath;
			}

			if (response[CookieName] != null) response.Set(Cookie);
			else
			{
				response.Add(Cookie);
			}
		}

		/// <summary>
		/// Loads object from request cookie.
		/// It uses the default parser to create the object
		/// </summary>
		/// <param name="request">Request cookie collection</param>
		public bool Load(HttpCookieCollection request)
		{
			return Load(request, new GenericStringParser<T>());
		}

		/// <summary>
		/// Loads object from request cookie, using the supplied parser
		/// </summary>
		/// <param name="request">Request cookie collection</param>
		/// <param name="parser">Parser to convert cookie value to object</param>
		/// <returns></returns>
		public bool Load(HttpCookieCollection request, IStringParser<T> parser)
		{
			if (string.IsNullOrEmpty(CookieName)) throw new InvalidOperationException("CookieName is not set");
			if (parser == null) throw new ArgumentNullException("parser");
			Cookie = request[CookieName];
			if (Cookie == null || string.IsNullOrEmpty(Cookie.Value)) return false;
			T value = default(T);
			
			if(parser.TryParse(Cookie.Value, out value))
			{
				Value = value;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Cleans up the cookie
		/// </summary>
		/// <param name="response">Response cookie collection</param>
		public void CleanUp(HttpCookieCollection response)
		{
			if (response == null) throw new ArgumentNullException("response");
			
			if (Cookie==null) return;//nothing to clean
			
			Cookie.Expires = new DateTime(2009, 1, 1);
			response.Add(Cookie);
		}

		/// <summary>
		/// Gets the underlaying cookie
		/// </summary>
		public HttpCookie Cookie { get; private set; }
	}
	
	
}