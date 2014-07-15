using System;
using System.Threading.Tasks;

namespace CavemanTools.Infrastructure
{
    public static class MessagingMediator
    {
        /// <summary>
        /// It should return instance or null
        /// </summary>
        public static Func<Type, object> Resolver = t => { throw new InvalidOperationException("There is no resolver set for the MessagingMediator. Use 'MessageMediator.Resolver=type=>{/* call container to return instance */}'");};

       
        /// <summary>
        /// Invokes the query handler which will take the specified argument as the input model
        /// </summary>
        /// <typeparam name="TOut">View model</typeparam>
        /// <param name="model">Input model</param>
        /// <returns></returns>
        public static TOut QueryTo<TOut>(this object model) where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQuery<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler = (dynamic)Resolver(handlerType);
            if (handler == null) throw new InvalidOperationException("There's no handler implementing 'IHandleQuery<{0},{1}>' registered with the DI Container".ToFormat(model.GetType().Name, typeof(TOut).Name));
            return (TOut)handler.Handle((dynamic)model);
        }
#if !Net4
        /// <summary>
        /// Invokes the async query handler which will take the specified argument as the input model
        /// </summary>
        /// <typeparam name="TOut">View model</typeparam>
        /// <param name="model">Input model</param>
        /// <returns></returns>
        public static Task<TOut> QueryAsyncTo<TOut>(this object model)  where TOut : class
        {
            model.MustNotBeNull();
            var handlerType = typeof(IHandleQueryAsync<,>).MakeGenericType(model.GetType(), typeof(TOut));
            var handler= (dynamic)Resolver(handlerType);
            if (handler==null) throw new InvalidOperationException("There's no handler implementing 'IHandleQueryAsync<{0},{1}>' registered with the DI Container".ToFormat(model.GetType().Name, typeof(TOut).Name));
            return (Task<TOut>) handler.HandleAsync((dynamic)model);
        }
#endif
        /// <summary>
        /// Invokes a request (command) taking the specified argument as the input model and returns its result
        /// </summary>
        /// <typeparam name="TResult">Output model</typeparam>
        /// <param name="input">Input model</param>
        /// <returns></returns>
        public static TResult ExecuteAndReturn<TResult>(this object input) where TResult : class
        {
            input.MustNotBeNull();
            var handlerType = typeof(IHandleCommand<,>).MakeGenericType(input.GetType(), typeof(TResult));
            var handler = (dynamic)Resolver(handlerType);
            if (handler == null) throw new InvalidOperationException("There's no handler implementing 'IHandleCommand<{0},{1}>' registered with the DI Container".ToFormat(input.GetType().Name, typeof(TResult).Name));
            return (TResult)handler.Handle((dynamic)input);
        }
#if !Net4        
        /// <summary>
        /// Invokes a request (command) taking the specified argument as the input model and returns its result
        /// </summary>
        /// <typeparam name="TResult">Output model</typeparam>
        /// <param name="input">Input model</param>
        /// <returns></returns>
        public static Task<TResult> ExecuteAsyncAndReturn<TResult>(this object input) where TResult : class
        {
            input.MustNotBeNull();
            var handlerType = typeof(IHandleCommandAsync<,>).MakeGenericType(input.GetType(), typeof(TResult));
            var handler = (dynamic)Resolver(handlerType);
            if (handler == null) throw new InvalidOperationException("There's no handler implementing 'IHandleCommand<{0},{1}>' registered with the DI Container".ToFormat(input.GetType().Name, typeof(TResult).Name));
            return (Task<TResult>)handler.Handle((dynamic)input);
        }
#endif
    }
}