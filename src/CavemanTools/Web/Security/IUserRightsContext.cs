// gg
using System.Collections.Generic;


namespace CavemanTools.Web.Security
{
    public interface IUserRightsContext
    {
        bool HasRightTo(byte right);
        int? UserId { get; }
        string Name { get; }
        
        bool IsMemberOf(IEnumerable<int> groupIds);
        bool IsAuthenticated { get; }
    }
}