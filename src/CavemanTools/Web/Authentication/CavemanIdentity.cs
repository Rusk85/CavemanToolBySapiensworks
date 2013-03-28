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


        public CavemanIdentity(IUserIdValue userId, IUserContextGroup grp) : base(userId, grp)
        {
        }

        public CavemanIdentity(IUserIdValue userId, IEnumerable<IUserContextGroup> groups) : base(userId, groups)
        {
        }

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