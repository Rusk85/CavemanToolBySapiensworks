using System.Web;
using CavemanTools.Strings;

namespace CavemanTools.Web
{
	/// <summary>
	/// Provides caching functionality for one value
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICookieCache<T>
	{
		/// <summary>
		/// Gets or sets object 
		/// </summary>
		T Value { get; set; }

		/// <summary>
		/// Gets the underlaying cookie
		/// </summary>
		HttpCookie Cookie { get; }

		/// <summary>
		/// Gets or sets cookie name which stores the value of parameter
		/// </summary>
		string CookieName { get; set; }

		/// <summary>
		/// Saves value into response cookie.
		/// Uses object's ToString()
		/// </summary>
		/// <param name="response">Response cookie collection</param>
		void Store(HttpCookieCollection response);

		/// <summary>
		/// Tries to load object from request cookie, using the supplied parser
		/// </summary>
		/// <param name="request">Request cookie collection</param>
		/// <param name="parser">Parser to convert cookie value to object</param>
		/// <returns></returns>
		bool Load(HttpCookieCollection request, IStringParser<T> parser);

		/// <summary>
		/// Cleans up the cookie
		/// </summary>
		/// <param name="response">Response cookie collection</param>
		void CleanUp(HttpCookieCollection response);
	}
}