using System;
using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public class UserContextGroup : IUserContextGroup
    {
        private IEnumerable<ushort> _rights;

        public UserContextGroup(int id,IEnumerable<ushort> rights)
        {
            if (rights == null) throw new ArgumentNullException("rights");
            Id = id;
            _rights = rights;                     
        }
        public int Id { get; private set; }
        public IEnumerable<ushort> Rights
        {
            get { return _rights; }
        }

        public AuthorizationScopeId ScopeId { get;  set; }
    }
}