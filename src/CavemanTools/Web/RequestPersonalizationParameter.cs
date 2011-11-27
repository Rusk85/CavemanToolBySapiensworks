using System;
using System.Web;
using CavemanTools.Strings;

namespace CavemanTools.Web
{
	/// <summary>
	/// Handles a request parameter which personalizes the request and can be temporary stored,
	/// such as a theme, language, number of items etc
	/// </summary>
	public class RequestPersonalizationParameter<T>
	{
		
		/// <summary>
		/// Initializes with default settings: generic string parser, cookie caching
		/// </summary>
		/// <param name="paraName"></param>
		public RequestPersonalizationParameter(string paraName)
			: this(HttpContext.Current.Request.Cookies,HttpContext.Current.Response.Cookies,new GenericStringParser<T>(),new CookieCaching<T>(){CookieName = "_"+paraName})
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response">Response cookie collection</param>
		/// <param name="parser">String parser to object</param>
		/// <param name="cookieCache">Caching implementation</param>
		/// <param name="request">Request cookie collection</param>
		public RequestPersonalizationParameter(HttpCookieCollection request,HttpCookieCollection response,IStringParser<T> parser, ICookieCache<T> cookieCache)
		{
			if (request == null) throw new ArgumentNullException("request");
			if (response == null) throw new ArgumentNullException("response");
			if (parser == null) throw new ArgumentNullException("parser");
			if (cookieCache == null) throw new ArgumentNullException("cookieCache");
			
			Parser = parser;
			Caching = cookieCache;	
			_request = request;
			_response = response;

		}

		/// <summary>
		/// Tries to load parameter from a string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool LoadFromString(string value)
		{
			if (string.IsNullOrEmpty(value)) return false;

			try
			{
				Value = Parser.Parse(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Tries to load parameter from cache
		/// </summary>
		/// <returns></returns>
		public bool LoadFromCache()
		{
			try
			{
				if (Caching.Load(_request, Parser))
				{
					Value = Caching.Value;
					return true;
				}
				
			}
			catch
			{
				
			}
			Caching.CleanUp(_response);
			return false;
		}

		/// <summary>
		/// Caches value using provided caching implementation
		/// </summary>
		public void Cache()
		{
			Caching.Value = Value;
			Caching.Store(_response);
		}

		/// <summary>
		/// Gets or sets the parameter value
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// Gets or sets caching storage object
		/// </summary>
		public ICookieCache<T> Caching { get; set; }
		protected IStringParser<T> Parser;
		private HttpCookieCollection _request;
		private HttpCookieCollection _response;
	}
}