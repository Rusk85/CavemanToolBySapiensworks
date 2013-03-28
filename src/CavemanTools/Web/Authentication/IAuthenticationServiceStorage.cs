using System;

namespace CavemanTools.Web.Authentication
{
    public interface IAuthenticationServiceStorage
    {
        /// <summary>
        /// Returns session data. If it's expired or missing returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AuthenticationData Get(Guid id);

        /// <summary>
        /// Stores session data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="expiration"></param>
        void Save(Guid id, AuthenticationData user, DateTimeOffset expiration);

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        void MarkAsFinished(Guid id);

    }
}