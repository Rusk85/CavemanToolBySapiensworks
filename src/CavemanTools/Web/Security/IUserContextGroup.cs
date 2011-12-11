using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public interface IUserContextGroup
    {
        int Id { get; }
       
        IEnumerable<ushort> Rights { get; }
    }
}