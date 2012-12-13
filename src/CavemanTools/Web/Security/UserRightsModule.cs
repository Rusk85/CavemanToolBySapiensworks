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
    public class UserRightsModule:IHttpModule
    {
        private Func<IUserRightsRepository> _repo;

        public const string ContextKey = "_usr-context_";
        public const string ContextScopeIdKey = "_usr-context_scope";

        public UserRightsModule(Func<IUserRightsRepository> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            _repo = repository;
        }
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += context_PostAuthenticateRequest;
        }

        void context_PostAuthenticateRequest(object sender, System.EventArgs e)
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
            if (authData == null)
            {
               return GetAnonymousContext(repo);
            }
            
            //get group
            var grp = repo.GetGroupsById(authData.Groups,authData.UserId);
            var usr = new UserRightsContext(authData.UserId, grp);
            usr.Name = fi.Name;
            SetScope(usr);
            return usr;
        }

        static void SetScope(IUserRightsContext usr)
        {
            var scope = HttpContext.Current.Get<AuthorizationScopeId>(ContextScopeIdKey);
            if (scope != null) usr.ScopeId = scope;
        }

        internal static IUserRightsContext GetAnonymousContext(IUserRightsRepository repo)
        {
           var rez= new UserRightsContext(null,repo.GetDefaultGroup());
            SetScope(rez);
            return rez;
        }

        public void Dispose()
        {
            
        }
    }

}