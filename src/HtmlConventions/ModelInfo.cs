using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcHtmlConventions
{
    public class ModelInfo
    {
        public ModelInfo(ModelMetadata meta)
        {
            if (meta.ContainerType != null)
            {
                PropertyDefinition = meta.ContainerType.GetProperty(meta.PropertyName);
                
                Name = meta.PropertyName;
                ParentType = meta.ContainerType;
            }
            else
            {
                IsRootModel = true;
                Name = meta.ModelType.Name;
            }
            RawValue = meta.Model;
            Type = meta.ModelType;
            
        }
        public PropertyInfo PropertyDefinition { get; private set; }
        public bool IsRootModel { get; private set; }
        public Type Type { get; private set; }

        public string Name
        {
            get; private set;
        }

        public string HtmlId { get; set; }
        
        public string HtmlName { get; set; }
        
        public object RawValue { get; private set; }

        public T Value<T>()
        {
            return (T) RawValue;
        }

        public Type ParentType
        {
            get; private set;
        }

    }
}