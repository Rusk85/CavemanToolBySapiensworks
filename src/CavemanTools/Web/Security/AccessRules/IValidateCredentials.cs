namespace CavemanTools.Web.Security.AccessRules
{
    public interface IValidateCredentials
    {
        bool HasValidCredentials(IUserRightsContext user);
    }
}