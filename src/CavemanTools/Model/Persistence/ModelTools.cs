using System;
using System.Threading;

namespace CavemanTools.Model.Persistence
{
    public class ModelTools
    {
        /// <summary>
        /// Executes the action which should update and store an entity.
        /// If action throws <see cref="NewerVersionExistsException"/> it's retried for the specified number of times.
        /// Returns a result if the action was executed successfully(regardless of the number of tries).
        /// </summary>
        /// <param name="update"></param>
        /// <param name="triesCount"></param>
        /// <returns></returns>
        public static bool TryUpdateEntity(Action update, int triesCount=10)
        {
            var i = 0;
            do
            {
                try
                {
                    update();
                    break;
                }
                catch (NewerVersionExistsException)
                {
                    i++;
                }
                
            } while (i < triesCount);
            return i < triesCount;
        }

        public static T GetBlocking<T>(Func<T> factory,int retries=10,int sleep=100) where T:class 
        {
            T data;
            var i = 0;
            do
            {
                data = factory();
                if (data != null) break;
                i++;
                Thread.Sleep(sleep);

            } while (i < retries);
            return data;
        }
    }
}