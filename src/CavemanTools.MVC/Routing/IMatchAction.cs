namespace CavemanTools.Mvc.Routing
{
    public interface IMatchAction
    {
        bool Match(ActionCall action);
    }
}