using System;
using System.IO;
using System.Net.Mail;
using CavemanTools.Strings;

namespace CavemanTools.Web.Email
{
	public static class EmailUtils
	{
		/// <summary>
		/// Uses a template to generate the message body.
		/// The template parameters have the format {Parameter}.
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="file">Template file path</param>
		/// <param name="values">Anonymous object with values for placeholders</param>
		public static void UseTemplate(this MailMessage msg, string file, object values)
		{
			if (msg == null) throw new ArgumentNullException("msg");
			var body = File.ReadAllText(file);
			msg.Body = body.HenriFormat(values);
		}

		/// <summary>
		/// Generates the Gravatar id from email.
		/// </summary>
		/// <param name="email">Email address</param>
		/// <returns></returns>
		public static string EmailToGravatar(this string email)
		{
			if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
			return email.ToLowerInvariant().MD5();
		}
	}
}