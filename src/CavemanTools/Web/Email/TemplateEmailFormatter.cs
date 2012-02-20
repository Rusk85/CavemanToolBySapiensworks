using System;
using System.IO;
using System.Net.Mail;

namespace CavemanTools.Web.Email
{
	/// <summary>
	/// This class formats a tempalte file into a Mail Message
	/// </summary>
	public class TemplateEmailFormatter
	{

		public TemplateEmailFormatter():this(new MailMessage())
		{
			
		}

		public TemplateEmailFormatter(MailMessage msg)
		{
			if (msg == null) throw new ArgumentNullException("msg");
			Message = msg;
		}

		/// <summary>
		/// Loads the template from file. Template uses {ParamName} for parameters
		/// </summary>
		/// <param name="file">File path</param>
		public void LoadFileTemplate(string file)
		{
			Template = File.ReadAllText(file);
		}

		/// <summary>
		/// Gets or sets the template string. Template uses {ParamName} for parameters.
		/// </summary>
		public string Template { get; set; }

		
		/// <summary>
		/// Creates the message body from template.
		/// </summary>
		/// <param name="values"></param>
		public void ProcessTemplate(object values)
		{
			if (string.IsNullOrEmpty(Template)) throw new InvalidOperationException("Can't process an empty template");
			Message.Body = Template.HenriFormat(values);
		}

		/// <summary>
		/// Gets the mail message with formatted body.
		/// </summary>
		public MailMessage Message { get; private set; }
	}
}