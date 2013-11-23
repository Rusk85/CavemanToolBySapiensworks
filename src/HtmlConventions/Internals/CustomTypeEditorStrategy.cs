using System;
using System.Web.Mvc;
using HtmlTags;

namespace MvcHtmlConventions.Internals
{
    class CustomTypeEditorStrategy : IGenerateHtml
    {
        private readonly ModelMetadata _meta;
        private readonly IHaveModelConventions _conventions;

        public CustomTypeEditorStrategy(ModelMetadata meta, IHaveModelConventions conventions)
        {
            _meta = meta;
            _conventions = conventions;
        }

        public HtmlTag GenerateElement(ModelInfo info)
        {
            var tag = HtmlTag.Placeholder();
            foreach (var property in _meta.Properties)
            {
                var child = GetTag(property, info);
                tag.Children.Add(child);
            }
            _conventions.Builder=new NopBuilder(tag);
            return _conventions.CreateGenerator().GenerateElement(info);
        }

        HtmlTag GetTag(ModelMetadata property,ModelInfo parentInfo)
        {
            var info=new ModelInfo(property);
            if (parentInfo.HtmlName.IsNullOrEmpty())
            {
                info.HtmlId=info.HtmlName = property.PropertyName;                 
            }

            else
            {
                info.HtmlId = parentInfo.HtmlId + "_" + info.Name;
                info.HtmlName = parentInfo.HtmlName + "." + info.Name;
            }
            var propConventions = _conventions.Registry.GetConventions(info);
            var propGenerator = ModelTypeAdviser.GetEditorGenerator(info, property,propConventions);
            return propGenerator.GenerateElement(info);
        }
    }
}