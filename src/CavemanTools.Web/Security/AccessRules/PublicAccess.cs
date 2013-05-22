
using CavemanTools.Web.Authentication;

namespace CavemanTools.Web.Security.AccessRules
{
    public class PublicAccess:IValidateCredentials
    {
        public virtual bool HasValidCredentials(IUserRightsContext user)
        {
            return true;
        }
    }
}