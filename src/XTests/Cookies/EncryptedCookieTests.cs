using System.Web;
using CavemanTools.Web;
using Xunit;

namespace XTests.Cookies
{
	public class EncryptedCookieTests
	{
		public EncryptedCookieTests()
		{

		}

		//[Fact]
		public void Default_Encription()
		{
			//var text = "a whatever value text";
			//var def = new DefaultCookieEncryption();
			//def.Secret = "12345678";
			//var encr = def.Encrypt(text);
			//Assert.NotEqual(text,encr);
			//var decr = def.Decrypt(encr);
			//Assert.Equal(text, decr);
		}

		[Fact]
		public void Usage()
		{
			var encr = new EncryptedCookie("est", "12345678");
			encr["role"] = "admin";
			encr["id"] = "2";
			var resp = new HttpCookieCollection();
			encr.Save(resp);

			var ck2 = new HttpCookie("d");
			ck2.Value = encr.Cookie.Value;

			var en2 = new EncryptedCookie(ck2, new DefaultCookieEncryption("12345678"));

			en2.Unseal();

			var id = en2.GetValue("id", -1);
			Assert.Equal(2, id);
			Assert.Equal("admin", en2["role"]);
		}

	//	[Fact]
		public void Usage_Empty()
		{
			var encr = new EncryptedCookie("est", "12345678");
		
			var resp = new HttpCookieCollection();
			encr.Save(resp);

			var ck2 = new HttpCookie("d");
			ck2.Value = encr.Cookie.Value;

			var en2 = new EncryptedCookie(ck2, new DefaultCookieEncryption("12345678"));

			en2.Unseal();

			var d = en2["id"];
		}
	}
}