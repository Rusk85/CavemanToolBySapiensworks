using HtmlTags;

namespace MvcHtmlConventions
{
    public interface IModifyElement:ISelectConvention
    {
        HtmlTag Modify(HtmlTag tag, ModelInfo info);
    }
}