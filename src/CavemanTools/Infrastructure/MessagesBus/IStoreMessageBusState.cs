using System;
using System.Collections.Generic;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IStoreMessageBusState
    {
        /// <summary>
        /// Stores and marks a message as in progress
        /// </summary>
        /// <exception cref="DuplicateMessageException"></exception>
        /// <param name="cmd"></param>
        void StoreMessageInProgress(IMessage cmd);
        void StoreMessageCompleted(Guid id);

        /// <summary>
        /// Gets all the un completed messages, usually as aresult of server crash
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMessage> GetUncompletedMessages();

        /// <summary>
        /// Do message store cleanup
        /// </summary>
        void Cleanup();

        ///// <summary>
        ///// The commands must be stored as in progress
        ///// </summary>
        ///// <param name="result"></param>
        //void StoreSagaResult(SagaResult result);
        //T GetSagaState<T>(string correlationId) where T:IContainSagaState;
    }

   

    //public class SagaResult
    //{
    //    public SagaResult()
    //    {
    //        Commands=new ICommand[0];
    //    }
    //    public IContainSagaState State { get; set; }
    //    public IEnumerable<ICommand> Commands { get; set; }
    //}


}