using HtmlTags;

namespace MvcHtmlConventions.Internals
{
    class NullStrategy : IGenerateHtml
    {
        public static IGenerateHtml Instance=new NullStrategy();
        public HtmlTag GenerateElement(ModelInfo info)
        {
            return HtmlTag.Empty();
        }
    }
}