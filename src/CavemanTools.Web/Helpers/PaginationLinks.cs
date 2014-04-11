using System;
using System.Collections.Generic;
using System.Web;

namespace CavemanTools.Web.Helpers
{
	/// <summary>
	/// Creates pagination links
	/// </summary>
	public class PaginationLinks
	{
	    public PaginationLinks()
	    {
	        CurrentPageFormat = i=>@"<span class=""current"">"+i+"</span>";
	    }
        private string _linkFormat;

        /// <summary>
        /// Gets or sets the url format for links.
        /// </summary>
        public Func<int, string> LinkUrlFormat { get; set; }

		/// <summary>
		/// Active page html element format. Default it renders
		/// &lt;span class="current"&gt;{0}&lt;span&gt;
		/// </summary>
		public Func<int,string> CurrentPageFormat { get; set;}
		/// <summary>
		/// Gets or sets the total results number
		/// </summary>
		public int TotalItems { get; set; }
		/// <summary>
		/// Gets or sets the number of results on a page
		/// </summary>
		public int ItemsOnPage { get; set; }
		/// <summary>
		/// Gets or sets the current page number
		/// </summary>
		public int Current { get; set; }
		
		/// <summary>
		/// Gets the links to use for pagination
		/// </summary>
		/// <returns></returns>
		public string[] GetPages()
		{
			if (LinkUrlFormat==null) throw new InvalidOperationException("You must set the LinkFormat property");
            var total = TotalToPages(TotalItems, ItemsOnPage);
			if (Current>total) Current = total;
			var l = new List<string>();
			var i = (int)Math.Ceiling((double) Current/10);
			if (i == 0) i = 1;
			if (i>1)
			{
				l.Add(FormatPageLink(1));
				l.Add(GetLinkTag((i-1)*10,"[...]"));
			}
			var ist = (i - 1)*10 + 1;
			var ei = i*10;
			if (ei>total) ei = total;
			for(int k=ist;k<=ei;k++) l.Add(FormatPageLink(k));

			var ti = (int)Math.Ceiling((double)total / 10);
			if (ti>i)
			{
                l.Add(GetLinkTag(i * 10+1, "[...]"));
                l.Add(FormatPageLink(total));
			}

			return l.ToArray();
		}

        string GetLinkTag(int i,string text)
        {
            return string.Format(@"<a href=""{0}"">{1}</a>", LinkUrlFormat(i), text);
        }

		string FormatPageLink(int i)
		{
			if (i==Current)
			{
				return CurrentPageFormat(i);
			}
		    return GetLinkTag(i, i.ToString());
		}

		/// <summary>
		/// Caculates the number of pages 
		/// </summary>
		/// <param name="total">Total items number</param>
		/// <param name="itemsOnPage">Items shown on a page</param>
		/// <returns></returns>
		public static int TotalToPages(int total, int itemsOnPage)
		{
			if (itemsOnPage == 0) return 0;
			return (int) Math.Ceiling((double) total/itemsOnPage);
		}
        
	}
}