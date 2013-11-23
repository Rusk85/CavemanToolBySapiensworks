using HtmlTags;

namespace MvcHtmlConventions.Internals
{
    abstract class BaseStrategy : IGenerateHtml
    {
      private readonly IHaveModelConventions _conventions;

        protected BaseStrategy(IHaveModelConventions conventions)
        {
            _conventions = conventions;
        }

        protected abstract void Configure(IHaveModelConventions conventions);
        public HtmlTag GenerateElement(ModelInfo info)
        {
            Configure(_conventions);
            return _conventions.CreateGenerator().GenerateElement(info);
        }
    }
}