using System;
using System.Collections.Generic;

namespace CavemanTools.Security
{
    public class UserContextGroup : IUserContextGroup
    {
        private IEnumerable<string> _rights;

        public UserContextGroup(int id,IEnumerable<string> rights)
        {
            if (rights == null) throw new ArgumentNullException("rights");
            Id = id;
            _rights = rights;
        }
        public int Id { get; private set; }
        public IEnumerable<string> Rights
        {
            get { return _rights; }
        }
    }
}