using System;

namespace MvcHtmlConventions
{
    public interface IConfigureActionCriteria
    {
        IConfigureAction If(Predicate<ModelInfo> info);
        IConfigureModifier Always { get; }
    }
}