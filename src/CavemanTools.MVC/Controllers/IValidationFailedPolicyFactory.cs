using System;

namespace CavemanTools.Mvc.Controllers
{
    public interface IValidationFailedPolicyFactory
    {
        IResultForInvalidModel<T> GetInstance<T>(Type policy, T model) where T : class, new();
    }
}