using System;
using System.Security;
using System.Web;
using CavemanTools.Strings;

namespace CavemanTools.Web
{
	public class DefaultCookieEncryption:ICookieEncryption
	{
		public DefaultCookieEncryption(string secret)
		{
			if (string.IsNullOrEmpty(secret) || secret.Length < 8)
				throw new InvalidOperationException("The secret should be at least 8 characters long");
			Secret = secret;
		}
		public void Encrypt(HttpCookie cookie)
		{
			if (cookie == null) throw new ArgumentNullException("cookie");
			if (string.IsNullOrEmpty(cookie.Value)) return;
			var encripted = cookie.Value.EncryptAsString(Secret);
			//no hashing by default to keep the cookie length smaller, not very safe, but good enough
			cookie.Value= encripted;
		}

		public void Decrypt(HttpCookie cookie)
		{
			if (cookie == null) throw new ArgumentNullException("cookie");
			if (string.IsNullOrEmpty(cookie.Value)) return;
			var text = "";
			try
			{
				text=cookie.Value.DecryptAsString(Secret);
			}
			catch
			{
				throw new SecurityException(string.Format("Invalid data: {0}",cookie.Value));
			}
			var values = HttpUtility.ParseQueryString(text);
			cookie.Values.Remove(null);
			foreach(var name in values.AllKeys)
			{
				cookie.Values[name] = values[name];
			}
			
		}


		/// <summary>
		/// At least 8 characters
		/// </summary>
		public string Secret { get; set; }
	}
}