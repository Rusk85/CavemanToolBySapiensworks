// gg

namespace CavemanTools.Security
{
    public interface IUserRightsContext
    {
        bool HasRight(string right);
        int? UserId { get; }
        string Name { get; }
        bool IsMemberOf(params int[] groupId);
        bool IsAuthenticated { get; }
    }

    
}