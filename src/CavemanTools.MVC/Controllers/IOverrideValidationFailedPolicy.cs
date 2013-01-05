namespace CavemanTools.Mvc.Controllers
{
   public interface IOverrideValidationFailedPolicy
    {
        IResultForInvalidModel<T> GetPolicy<T>(T model) where T : class, new();
    }


}