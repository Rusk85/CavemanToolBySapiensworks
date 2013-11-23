using System;
using System.Collections;
using System.Web.Mvc;

namespace MvcHtmlConventions.Internals
{
    internal class ModelTypeAdviser
    {
        
        public static IGenerateHtml GetEditorGenerator(ModelInfo info, ModelMetadata meta, IHaveModelConventions conventions)
        {
            if (conventions.Builder != null)
            {
                return conventions.CreateGenerator();
            }
            if (info.Type.IsUserDefinedClass())
            {
                return new CustomTypeEditorStrategy(meta,conventions);                
            }
            if (info.Type==typeof(string) || !info.Type.DerivesFrom<IEnumerable>())
            {
                return new PrimitiveTypeEditorStrategy(conventions);
            }
            return NullStrategy.Instance;
        }
        
    }
}