namespace CavemanTools.Model
{
    /// <summary>
    /// Interface for command handlers
    /// </summary>
    /// <typeparam name="TCommand">A reference type</typeparam>
    public interface IHandleCommand<TCommand> where TCommand:class
    {
        /// <summary>
        /// Handle command
        /// <exception cref="BusinessRuleException"></exception>
        /// </summary>
        /// <param name="command"></param>
        void Handle(TCommand command);
    }

    /// <summary>
    /// Interface for command handler when you also need to return a response to the user
    /// </summary>
    /// <typeparam name="TCommand">reference type</typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IHandleRequest<in TCommand, out TResult> where TCommand:class 
    {
        /// <summary>
        /// Handle command 
        /// <exception cref="BusinessRuleException"></exception>
        /// </summary>
        /// <returns></returns>
        TResult  Process(TCommand command);
    }

   
}