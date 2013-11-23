namespace MvcHtmlConventions.Internals
{
    class PrimitiveTypeEditorStrategy : BaseStrategy
    {
        public PrimitiveTypeEditorStrategy(IHaveModelConventions conventions) : base(conventions)
        {
        }

        protected override void Configure(IHaveModelConventions conventions)
        {
            conventions.Builder = conventions.Registry.GetDefaultBuilder();
        }
    }
}