using System.Globalization;
using System.Web;
using CavemanTools.Logging;
using CavemanTools.Web.Localization;
using Xunit;

namespace XTests.Web_Request
{
	public class RequestLocaleTests
	{
		private RequestLocale _locale;
		private HttpCookieCollection _req;
		private HttpCookieCollection _resp;

		public ILogWriter Log { get; set; }
		
		public void SomeMehtod()
		{
			Log.Write(LogLevel.Debug,"debugging info");
		}

		public RequestLocaleTests()
		{
			_req = new HttpCookieCollection();
			_resp = new HttpCookieCollection();
			_locale = new RequestLocale(_req, _resp);
			
		}


		[Fact]
		public void Request_Locale_From_string()
		{
			Assert.True(_locale.LoadFromString("en"));
			Assert.Equal(new CultureInfo("en"),_locale.Value);
		}

		[Fact]
		public void Load_From_Cache_Ok()
		{
			var ck = new HttpCookie("_locale", "fr");
			_req.Add(ck);
			Assert.True(_locale.LoadFromCache());
			Assert.Equal(new CultureInfo("fr"),_locale.Value);
		}


		[Fact]
		public void Save_cache_Ok()
		{
			_locale.Value=new CultureInfo("fr");
			_locale.Cache();
			Assert.NotNull(_resp["_locale"]);
		}

		[Fact]
		public void Request_Cookie_Doesnt_Exist()
		{
			Assert.False(_locale.LoadFromCache());
			Assert.Null(_resp["_locale"]);
		}

		[Fact]
		public void Invalid_Request_Cookie()
		{
			var ck = new HttpCookie("_locale", "empty");
			_req.Add(ck);
			Assert.False(_locale.LoadFromCache());
			var clean = _resp["_locale"];
			Assert.NotNull(clean);
			Assert.Equal(clean.Expires.Year,2009);
		}
	}

}