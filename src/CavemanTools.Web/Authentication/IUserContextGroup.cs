using System.Collections.Generic;
using CavemanTools.Web.Security;

namespace CavemanTools.Web.Authentication
{
    public interface IUserContextGroup
    {
        /// <summary>
        /// Gets the group id value
        /// </summary>
        int Id { get; }
       
        /// <summary>
        /// Gets the rights assigned to group
        /// </summary>
        IEnumerable<ushort> Rights { get; }

        bool ContainsRights(params ushort[] right);

        /// <summary>
        /// Gets group scope if any
        /// </summary>
        AuthorizationScopeId ScopeId { get; }
    }
}