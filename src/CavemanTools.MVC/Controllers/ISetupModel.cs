namespace CavemanTools.Mvc.Controllers
{
    public interface ISetupModel<T> where T:class ,new()
    {
        void Setup(T model);
    }
}