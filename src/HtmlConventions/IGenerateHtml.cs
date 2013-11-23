using HtmlTags;

namespace MvcHtmlConventions
{
    public interface IGenerateHtml
    {
        HtmlTag GenerateElement(ModelInfo info);
        
    }
}