using System.Collections.Generic;

namespace CavemanTools.Web.Authentication
{
    public class AuthenticationData
    {
        public AuthenticationData(string name,IUserIdValue userId,IEnumerable<IUserContextGroup> userGroups=null)
        {
            Name = name;
            UserId = userId;
            if (userGroups == null)
            {
                Groups=new IUserContextGroup[0];
            }
            else
            {
                Groups = userGroups;
            }
        }

        public string Name { get; private set; }
        public IUserIdValue UserId { get; private set; }
        
        /// <summary>
        /// Groups user belongs to. Empty for none
        /// </summary>
        public IEnumerable<IUserContextGroup> Groups { get; private set; }        

    }
}