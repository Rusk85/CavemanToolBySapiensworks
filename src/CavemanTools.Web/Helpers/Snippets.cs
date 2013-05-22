namespace CavemanTools.Web.Helpers
{
	public static class Snippets
	{
		/// <summary>
		/// Returns analytics js code for id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string GoogleAnalytics(string id)
		{
			return
				string.Format(
					@"<script type=""text/javascript"">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', '{0}']);
  _gaq.push(['_trackPageview']);

  (function() {{
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  }})();

</script>",id);
		}
	}
}