using System;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Web.Security
{
    /// <summary>
    /// Change the AdminRight field to the value you want
    /// </summary>
    public class UserRightsContext:IUserRightsContext
    {
        private readonly IUserContextGroup _group;

        /// <summary>
        /// Value of Admin right.
        /// Default is UserBasicRights.Administration
        /// </summary>
        public static byte AdminRight= UserBasicRights.Administration;

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
        public bool HasRightTo(byte right)
        {
            return _group.Rights.Any(r => r==AdminRight || r==right);
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
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public bool IsMemberOf(IEnumerable<int> groupIds)
        {
            return groupIds.Any(d=>d==_group.Id);
        }

        public bool IsAuthenticated
        {
            get { return UserId != null; }
        }
    }
}