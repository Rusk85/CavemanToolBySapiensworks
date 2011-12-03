using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public interface IUserContextGroup
    {
        int Id { get; }
       
        IEnumerable<byte> Rights { get; }
    }
}