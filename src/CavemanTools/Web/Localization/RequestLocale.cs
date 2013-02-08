using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;


namespace CavemanTools.Web.Localization
{
	/// <summary>
	/// Establish UI Culture for current request
	/// </summary>
	/// <example>
	/// var req=new RequestLocale(Request.Cookies,Response.Cookies);
	/// if (req.LoadFromString(Request.QueryString["lang"]))
	///		{
	///			req.Cache();
	///		}
	///		else
	///		{
	///			req.LoadFromCache();
	///		}
	///
	///	if (req.Value!=null) Thread.CurrentThread.CurrentCulture = req.Value
	/// </example>
	public class RequestLocale:RequestPersonalizationParameter<CultureInfo>
	{
		public RequestLocale():this(HttpContext.Current.Request.Cookies,HttpContext.Current.Response.Cookies)
		{
			
		}
		public RequestLocale(HttpCookieCollection request,HttpCookieCollection response):base(request,response,new GenericStringParser<CultureInfo>(),new CookieCache<CultureInfo>(){CookieName = "_locale"})
		{
			
		}
		
		/// <summary>
		/// Sets the UICulture of the thread with the value if not empty
		/// </summary>
		public void ApplyLanguage()
		{
			if (Value==null) return;
			Thread.CurrentThread.CurrentUICulture = Value;
		}
	}
}