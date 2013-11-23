using HtmlTags;

namespace MvcHtmlConventions
{
    public interface IBuildElement : ISelectConvention
    {
        HtmlTag Build(ModelInfo info);
    }

    
}