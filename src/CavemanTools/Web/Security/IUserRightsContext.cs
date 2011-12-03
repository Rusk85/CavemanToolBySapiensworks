// gg

namespace CavemanTools.Web.Security
{
    public interface IUserRightsContext
    {
        bool HasRightTo(byte right);
        int? UserId { get; }
        string Name { get; }
        bool IsMemberOf(params int[] groupId);
        bool IsAuthenticated { get; }
    }
}