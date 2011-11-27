using System.Collections.Generic;

namespace CavemanTools.Security
{
    public interface IUserContextGroup
    {
        int Id { get; }
       
        IEnumerable<string> Rights { get; }
    }
}