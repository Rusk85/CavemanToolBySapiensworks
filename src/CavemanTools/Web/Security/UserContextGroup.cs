using System;
using System.Collections.Generic;

namespace CavemanTools.Web.Security
{
    public class UserContextGroup : IUserContextGroup
    {
        private IEnumerable<byte> _rights;

        public UserContextGroup(int id,IEnumerable<byte> rights)
        {
            if (rights == null) throw new ArgumentNullException("rights");
            Id = id;
            _rights = rights;                     
        }
        public int Id { get; private set; }
        public IEnumerable<byte> Rights
        {
            get { return _rights; }
        }
    }
}