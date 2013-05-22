using System;
using System.Collections.Generic;
using CavemanTools.Web.Authentication;

namespace CavemanTools.Web.Security.AccessRules
{
    public class MustBeInGroupAccess:IValidateCredentials
    {
        private int[] _groups;

        public MustBeInGroupAccess(params int[] groups)
        {
            if (groups == null) throw new ArgumentNullException("groups");
            _groups = groups;
        }

        /// <summary>
        /// Gets groups ids
        /// </summary>
        public IEnumerable<int> Groups
        {
            get { return _groups; }
        }

        public virtual bool HasValidCredentials(IUserRightsContext user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return user.IsMemberOf(_groups) || user.HasRightTo(UserBasicRights.DoEverything);
        }
    }
}