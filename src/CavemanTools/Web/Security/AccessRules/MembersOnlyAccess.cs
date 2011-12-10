using System;

namespace CavemanTools.Web.Security.AccessRules
{
    public class MembersOnlyAccess:IValidateCredentials
    {
        public virtual bool HasValidCredentials(IUserRightsContext user)
        {
            if (user == null) throw new ArgumentNullException("user");
            return user.IsAuthenticated;
        }
    }
}