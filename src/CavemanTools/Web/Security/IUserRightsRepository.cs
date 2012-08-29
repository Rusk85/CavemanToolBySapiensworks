﻿using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public interface IUserRightsRepository
    {
        /// <summary>
        /// Gets groups contexts. If a group doesn't exist, it must return an empty object
        /// </summary>
        /// <param name="ids">list of ids of groups</param>
        /// <param name="userId">user id from current request</param>
        /// <returns></returns>
        IEnumerable<IUserContextGroup> GetGroupsById(IEnumerable<int> ids,IUserIdValue userId=null);

        /// <summary>
        /// Returns the default group for anonymous users
        /// </summary>
        /// <returns></returns>
        IUserContextGroup GetDefaultGroup();
    }
}