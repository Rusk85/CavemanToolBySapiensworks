// gg

using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public interface IUserRightsRepository
    {
        /// <summary>
        /// Gets group context. If it doesn't exist, it must return an empty object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<IUserContextGroup> GetGroupsById(IEnumerable<int> ids);

        /// <summary>
        /// Returns the default group for anonymous users
        /// </summary>
        /// <returns></returns>
        IUserContextGroup GetDefaultGroup();
    }
}