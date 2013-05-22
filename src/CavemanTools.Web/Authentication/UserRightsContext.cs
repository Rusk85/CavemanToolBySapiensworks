using System;
using System.Collections.Generic;
using System.Linq;
using CavemanTools.Web.Security;

namespace CavemanTools.Web.Authentication
{
    /// <summary>
    /// Change the AdminRight field to the value you want
    /// </summary>
    public class UserRightsContext:IUserRightsContext
    {
        private readonly IEnumerable<IUserContextGroup> _groups;

        /// <summary>
        /// Value of Admin right.
        /// Default is UserBasicRights.DoEverything
        /// </summary>
        public static ushort AdminRight= UserBasicRights.DoEverything;

        public UserRightsContext(IUserIdValue userId,IUserContextGroup grp):this(userId,new[]{grp})
        {
            
        }
        public UserRightsContext(IUserIdValue userId,IEnumerable<IUserContextGroup> groups)
        {
            if (groups == null) throw new ArgumentNullException("groups");
            Id = userId;
            _groups = groups;
         
        }

        /// <summary>
        /// Checks if user has the specified right or the admin right
        /// </summary>
        /// <param name="right">right id</param>
        /// <returns></returns>
        public bool HasRightTo(ushort right)
        {
            return GetValidGroups().Any(g => g.ContainsRights(right, AdminRight));
        }

        IEnumerable<IUserContextGroup> GetValidGroups()
        {
            if (ScopeId == null) return _groups.Where(g => g.ScopeId == null);
            return _groups.Where(g => g.ScopeId == null || ScopeId.Equals(g.ScopeId));
        }

        /// <summary>
        /// Gets the user id or null if anonymous
        /// </summary>
        public IUserIdValue Id { get; private set; }

        /// <summary>
        /// User name or empty if anonymous
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if user is member of one of the specified groups
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public bool IsMemberOf(params int[] groupIds)
        {
            return groupIds.Any(d=>_groups.Any(g=>g.Id==d));
        }

        
        public bool IsAuthenticated
        {
            get { return Id != null; }
        }
        
        Dictionary<string,object> _other= new Dictionary<string, object>();

        /// <summary>
        /// Gets a dictionary where you can store other information about the user
        /// </summary>
        public IDictionary<string, object> OtherData
        {
            get { return _other; }
        }

      
        private AuthorizationScopeId _scope;

        public AuthorizationScopeId ScopeId
        {
            get { return _scope; }
            set
            {
                _scope = value;
            }
        }
    }
}