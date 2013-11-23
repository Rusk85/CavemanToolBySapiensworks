using System.Linq;
using System.Reflection;
using HtmlTags;

namespace MvcHtmlConventions
{
    public static class HtmlTagsExtentions
    {
        public static T GetChild<T>(this HtmlTag tag) where T:HtmlTag
        {
            var tp = typeof (T);
            return (T)tag.Children.FirstOrDefault(t => ReflectionUtils.IsExactlyType<T>(t));
        }            
    }
}