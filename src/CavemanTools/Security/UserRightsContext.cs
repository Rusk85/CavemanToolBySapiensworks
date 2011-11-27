using System;
using System.Linq;

namespace CavemanTools.Security
{
    /// <summary>
    /// Change the AdminRight field to the value you want
    /// </summary>
    public class UserRightsContext:IUserRightsContext
    {
        private readonly IUserContextGroup _group;

        /// <summary>
        /// Value of Admin right
        /// </summary>
        public static string AdminRight= "admin";

        public UserRightsContext(int? userId,IUserContextGroup group)
        {
            if (group == null) throw new ArgumentNullException("group");
            UserId = userId;
            _group = group;
        }

        /// <summary>
        /// Checks if user has the specified right or the admin right
        /// </summary>
        /// <param name="right">right id</param>
        /// <returns></returns>
        public bool HasRight(string right)
        {
            return _group.Rights.Any(r => r.Equals(AdminRight,StringComparison.InvariantCultureIgnoreCase) || r.Equals(right,StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the user id or null if anonymous
        /// </summary>
        public int? UserId { get; private set; }

        /// <summary>
        /// User name or empty if anonymous
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if user is member of one of the specified groups
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool IsMemberOf(params int[] groupId)
        {
            return groupId.Contains(_group.Id);
        }

        public bool IsAuthenticated
        {
            get { return UserId != null; }
        }
    }
}