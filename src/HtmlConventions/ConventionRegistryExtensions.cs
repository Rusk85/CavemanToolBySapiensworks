using System;

namespace MvcHtmlConventions
{
    public static class ConventionRegistryExtensions
    {
        public static IConfigureAction Unless(this IConfigureConventions data,Predicate<ModelInfo> predicate)
        {
            return data.If(d => !predicate(d));
        }
    }
}