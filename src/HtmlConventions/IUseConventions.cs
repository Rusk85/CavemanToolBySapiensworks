namespace MvcHtmlConventions
{
    public interface IUseConventions
    {
        
        IHaveModelConventions GetConventions(ModelInfo info);

        IBuildElement GetDefaultBuilder();
    }
}