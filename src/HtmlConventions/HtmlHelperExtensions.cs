using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTags;
using HtmlTags.Extended.Attributes;
using MvcHtmlConventions.Internals;

namespace MvcHtmlConventions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlTag InputFor<T,R>(this HtmlHelper<T> html, Expression<Func<T, R>> property)
        {
            var metadata = ModelMetadata.FromLambdaExpression(property, html.ViewData);
            var info = new ModelInfo(metadata);
            info.HtmlName = ExpressionHelper.GetExpressionText(property);
            info.HtmlId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(info.HtmlName);
            return RenderInputInfo(info, metadata);            
        }

        public static HtmlTag InputForModel<T>(this HtmlHelper<T> html)
        {
            var dt = html.ViewData.ModelMetadata;
            var info = new ModelInfo(dt);
            return RenderInputInfo(info, dt);
        }

        static HtmlTag RenderInputInfo(ModelInfo info, ModelMetadata meta)
        {
            var all = HtmlConventions.Instance.Editors.GetConventions(info);
            IGenerateHtml generator = ModelTypeAdviser.GetEditorGenerator(info, meta, all);
            var tag= generator.GenerateElement(info);
            return tag;
        }
    }
}