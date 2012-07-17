using System;

namespace CavemanTools.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBufferMessages:IDisposable
    {
        /// <summary>
        /// Sends all buffered messages
        /// </summary>
        void Flush();
    }
}