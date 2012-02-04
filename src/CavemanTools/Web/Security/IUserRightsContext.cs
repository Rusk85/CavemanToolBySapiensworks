// gg

using System.Collections.Generic;


namespace CavemanTools.Web.Security
{
    public interface IUserRightsContext
    {
        /// <summary>
        /// True if user has the specified right
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        bool HasRightTo(ushort right);
        /// <summary>
        /// Gets the user id or null if anonymous
        /// </summary>
        IUserIdValue Id { get; }
        /// <summary>
        /// User name or null if anonymous
        /// </summary>
        string Name { get; }
        /// <summary>
        /// True if the user is a member of at least one of the groups
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        bool IsMemberOf(params int[] groupIds);
      
        /// <summary>
        /// Gets the status of request
        /// </summary>
        bool IsAuthenticated { get; }
        /// <summary>
        /// Gets a dictionary where you can store other information about the user
        /// </summary>
        IDictionary<string, object> OtherData { get; }        
    }
}