﻿using System;
using HtmlTags;

namespace MvcHtmlConventions
{
    public interface IConfigureModifier
    {
        IConfigureConventions Modify(Func<HtmlTag, ModelInfo, HtmlTag> action);
    }

    public interface IConfigureAction : IConfigureModifier
    {
        IConfigureModifier Build(Func<ModelInfo, HtmlTag> action);
    }
}