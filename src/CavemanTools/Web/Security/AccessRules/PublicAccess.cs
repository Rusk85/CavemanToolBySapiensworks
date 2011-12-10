
namespace CavemanTools.Web.Security.AccessRules
{
    public class PublicAccess:IValidateCredentials
    {
        public bool HasValidCredentials(IUserRightsContext user)
        {
            return true;
        }
    }
}