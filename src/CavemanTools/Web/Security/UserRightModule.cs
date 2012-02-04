using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CavemanTools.Web.Security
{
    /// <summary>
    /// Requires an utility (like HttpModuleMagic.Mvc3 nuget package) for module dependecy injection.
    /// Automatically creates UserContext
    /// </summary>
    public class UserRightModule:IHttpModule
    {
        private Func<IUserRightsRepository> _repo;

        public const string ContextKey = "_usr-context_";

        public UserRightModule(Func<IUserRightsRepository> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repo = repository;
        }
        public void Init(HttpApplication context)
        {
            context.PostAuthorizeRequest += context_PostAuthorizeRequest;
        }

        void context_PostAuthorizeRequest(object sender, System.EventArgs e)
        {
            var ctx = (sender as HttpApplication).Context;
            if (ctx.Request.MatchesStaticResource()) return;
            CreateUserRightContext(ctx,_repo());
        }

        public static void CreateUserRightContext(HttpContext ctx,IUserRightsRepository repo)
        {
            ctx.Items[ContextKey] = (ctx.User.Identity.IsAuthenticated) ? GetUserContext(ctx.User.Identity, repo) : GetAnonymousContext(repo);        
        }

        internal static IUserRightsContext GetUserContext(IIdentity identity,IUserRightsRepository repo)
        {
            var fi = identity as FormsIdentity;
            if (fi == null) throw new NotSupportedException("Only FormsAuthentication is supported");
            var authData = AuthenticationUtils.Unpack(fi.Ticket.UserData);
            if (authData == null) return GetAnonymousContext(repo);
            
            //get group
            var grp = repo.GetGroupsById(authData.Groups);
            var usr = new UserRightsContext(authData.UserId,grp);
            usr.Name = fi.Name;
            return usr;
        }

        internal static IUserRightsContext GetAnonymousContext(IUserRightsRepository repo)
        {
           return new UserRightsContext(null,repo.GetDefaultGroup());
        }

        public void Dispose()
        {
            
        }
    }

}