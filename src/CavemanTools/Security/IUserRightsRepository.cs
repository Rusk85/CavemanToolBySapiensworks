// gg

namespace CavemanTools.Security
{
    public interface IUserRightsRepository
    {
        /// <summary>
        /// Gets group context. If it doesn't exist, it must return an empty object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IUserContextGroup GetGroupBytId(int id);

        /// <summary>
        /// Returns the default group for anonymous users
        /// </summary>
        /// <returns></returns>
        IUserContextGroup GetDefaultGroup();
    }
}