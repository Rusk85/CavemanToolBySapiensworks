using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IUnitOfWork:IDisposable
    {
        void Commit();
        string Tag { get; }
    }
}