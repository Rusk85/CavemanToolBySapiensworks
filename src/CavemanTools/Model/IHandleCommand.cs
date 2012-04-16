namespace CavemanTools.Model
{
    /// <summary>
    /// Interface for command handlers
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    public interface IHandleCommand<T> where T:class
    {
        void Handle(T command);
    }
}