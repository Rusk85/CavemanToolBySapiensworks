using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace CavemanTools.Web.Authentication
{
    public class CavemanIdentity: UserRightsContext,IIdentity
    {
        /// <summary>
        /// Authentication type
        /// </summary>
        public const string Type = "Caveman";

        /// <summary>
        /// Anonymous
        /// </summary>
        public CavemanIdentity():base(null,new IUserContextGroup[0])
        {
            SessionId = Guid.Empty;
        }

        public CavemanIdentity(Guid id,IUserIdValue userId, IUserContextGroup grp) : base(userId, grp)
        {
            SessionId = id;
        }

        public CavemanIdentity(Guid id,IUserIdValue userId, IEnumerable<IUserContextGroup> groups) : base(userId, groups)
        {
            SessionId = id;
        }

        public Guid SessionId { get; private set; }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <returns>
        /// The type of authentication used to identify the user.
        /// </returns>
        public string AuthenticationType
        {
            get { return Type; }
        }
    }
}