namespace CavemanTools.Web.Security.AccessRules
{
    public class MembersOnlyAccess:IValidateCredentials
    {
        public virtual bool HasValidCredentials(IUserRightsContext user)
        {
            return user.IsAuthenticated;
        }
    }
}