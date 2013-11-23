using System;
using HtmlTags;

namespace MvcHtmlConventions
{
    public interface IConfigureConventions:IConfigureActionCriteria
    {
        IConfigureConventions Add(IBuildElement builder);
        IConfigureConventions Add(IModifyElement modifier);
        IConfigureConventions DefaultBuilder(Func<ModelInfo,HtmlTag> action);
    }
}