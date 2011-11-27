using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CavemanTools.Lists;
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

         //public static MvcHtmlString Pager(this HtmlHelper html, int page,int itemsOnPage,int totalItems, string action,string controller,object routeValues,object htmlAttr,
         //                                         string currentFormat = "<span class=\"selected\">{0}</span>",string ulClass="")
         //{
         //    return Pager(html,page,itemsOnPage,totalItems,html.ActionLink("{0}",action,controller,routeValues,htmlAttr),)
         //}

        public static MvcHtmlString Pager(this HtmlHelper html, int page,int itemsOnPage,int totalItems, string linkFormat,
                                                  string currentFormat = "<span class=\"selected\">{0}</span>",string ulClass="")
        {
            var pl = new PaginationLinks();
            pl.Current = page;
            pl.ItemsOnPage = itemsOnPage;
            pl.TotalItems = totalItems;
            pl.LinkFormat = linkFormat;
            pl.CurrentFormat = currentFormat;
            var sb = new StringBuilder();
            sb.AppendFormat("<ul class=\"pager {0}\">",ulClass);
            foreach (var link in pl.GetPages())
            {
                sb.AppendFormat("<li>{0}</li>", link);
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }

    }
}