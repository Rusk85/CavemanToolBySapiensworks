using System.Collections.Generic;

namespace CavemanTools.Web.Security
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

        AuthorizationScopeId ScopeId { get; }
    }
}