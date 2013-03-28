using System;
using System.Security.Principal;
using CavemanTools.Infrastructure;

namespace CavemanTools.Web.Authentication
{
    public  class AuthenticationService:IAuthenticationService
    {
        private readonly ICacheData _cache;
        private readonly IAuthenticationServiceStorage _storage;

        public static TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public AuthenticationService(ICacheData cache,IAuthenticationServiceStorage storage)
        {
            cache.MustNotBeNull();
            storage.MustNotBeNull();
            _cache = cache;
            _storage = storage;
        }

        static string GenerateCacheKey(Guid id)
        {
            return id+"-auth";
        }

        /// <summary>
        /// If the guid is invalid (session expired or not found), it returns null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IIdentity Get(Guid id)
        {
            var key = GenerateCacheKey(id);
            var expiration = DateTimeOffset.Now.Add(CacheDuration);
            var data = _cache.Get<AuthenticationData>(key);
            if (data == null)
            {
                //get userdata
                data = _storage.Get(id);
                if (data == null)
                {
                    _cache.Remove(key);
                    return null;
                }
                _cache.Set(key, data, CacheDuration);
            }

            return CreateIdentity(data);
        }

        static CavemanIdentity CreateIdentity(AuthenticationData data)
        {
            var user = new CavemanIdentity(data.UserId, data.Groups);
            user.Name = data.Name;
            return user;
        }

        /// <summary>
        /// Returns the login session id
        /// </summary>
        /// <param name="data"></param>
        /// <param name="valability">Duration of the session</param>
        /// <returns></returns>
        public Guid StartSession(AuthenticationData data, TimeSpan valability )
        {
            var id = Guid.NewGuid();
            
            _storage.Save(id,data,DateTimeOffset.Now.Add(valability));
            _cache.Set(GenerateCacheKey(id), data, CacheDuration);
            return id;
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="id"></param>
        public void EndSession(Guid id)
        {
            _storage.MarkAsFinished(id);
            _cache.Remove(GenerateCacheKey(id));
        }
    }
}