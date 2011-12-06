using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using CavemanTools.Web.Helpers;
using System.Web.Mvc.Html;


namespace CavemanTools.Mvc.Extensions
{
    public static class HtmlHelpers
    {
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> source,Func<T,SelectListItem> lambda)
        {
            if (source==null || !source.Any()) yield break;
            foreach (var x in source) yield return lambda(x);
        }
     
        /// <summary>
        /// Creates page navigation links as an ul with CSS class=pager
        /// </summary>
        /// <param name="html">helper</param>
        /// <param name="page">Current page</param>
        /// <param name="itemsOnPage">Number of items displayed on a page</param>
        /// <param name="totalItems">Total number of items available</param>
        /// <param name="linkFormat">link format for paging navigation - {1} is page number to be displayed, {0} is page number used in the url</param>
        /// <param name="currentFormat">format for current page navigation</param>
        /// <param name="ulClass">additional CSS class for the ul</param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper html, int page,int itemsOnPage,int totalItems, string linkFormat,
                                                  string currentFormat=null,string ulClass="")
        {
            var pl = new PaginationLinks();
            pl.Current = page;
            pl.ItemsOnPage = itemsOnPage;
            pl.TotalItems = totalItems;
            pl.LinkFormat = linkFormat;
            pl.CurrentFormat = currentFormat??pl.CurrentFormat;
            var sb = new StringBuilder();
            sb.AppendFormat("<ul class=\"pager {0}\">",ulClass);
            foreach (var link in pl.GetPages())
            {
                sb.AppendFormat("<li>{0}</li>", link);
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="text"></param>
        /// <param name="selector">lambda to choose route values</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLinkTo<T>(this HtmlHelper html,string text,Expression<Func<T,object>> selector,object htmlAttributes=null) where T:Controller
        {
            return html.RouteLink(text, ControllerExtensions.ToRouteValues(selector), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
    }
}