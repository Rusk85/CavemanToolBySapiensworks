using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using CavemanTools.Web.Security;

namespace CavemanTools.Mvc.Security
{
    /// <summary>
    /// Use this with the HttpModuleMagic.Mvc3 nuget package for dependecy injection
    /// </summary>
    public class UserRightModule:IHttpModule
    {
        private IUserRightsRepository _repo;

        public UserRightModule(IUserRightsRepository repository)
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
            ctx.Items["_usr-context_"]=(ctx.User.Identity.IsAuthenticated)?GetUserContext(ctx.User.Identity):GetAnonymousContext();        
        }

        private IUserRightsContext GetUserContext(IIdentity identity)
        {
            var fi = identity as FormsIdentity;
            if (fi == null) throw new NotSupportedException("Only FormsAuthentication is supported");
            var authData = fi.Ticket.UserData.Unpack();
            if (authData == null) return GetAnonymousContext();
            
            //get group
            var grp = _repo.GetGroupById(authData.GroupId);
            var usr = new UserRightsContext(authData.Id,grp);
            usr.Name = fi.Name;
            return usr;
        }

        IUserRightsContext GetAnonymousContext()
        {
           return new UserRightsContext(null,_repo.GetDefaultGroup());
        }

        public void Dispose()
        {
            
        }
    }
}