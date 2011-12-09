using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CavemanTools.Web.Security;
using CavemanTools.Web;

namespace CavemanTools.Mvc.Security
{
    /// <summary>
    /// Use this with the HttpModuleMagic.Mvc3 nuget package for dependecy injection.
    /// Automatically creates UserContext
    /// </summary>
    public class UserRightModule:IHttpModule
    {
        private Func<IUserRightsRepository> _repo;

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

        internal static void CreateUserRightContext(HttpContext ctx,IUserRightsRepository repo)
        {
            ctx.Items["_usr-context_"] = (ctx.User.Identity.IsAuthenticated) ? GetUserContext(ctx.User.Identity, repo) : GetAnonymousContext(repo);        
        }

        internal static IUserRightsContext GetUserContext(IIdentity identity,IUserRightsRepository repo)
        {
            var fi = identity as FormsIdentity;
            if (fi == null) throw new NotSupportedException("Only FormsAuthentication is supported");
            var authData = fi.Ticket.UserData.Unpack();
            if (authData == null) return GetAnonymousContext(repo);
            
            //get group
            var grp = repo.GetGroupById(authData.GroupId);
            var usr = new UserRightsContext(authData.Id,grp);
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

    /// <summary>
    /// Asp.net mvc 3 global filter which creates UserContext
    /// </summary>
    public sealed class CavemanUserContext:IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserRightModule.CreateUserRightContext(HttpContext.Current, DependencyResolver.Current.GetService<IUserRightsRepository>());
        }
    }
}