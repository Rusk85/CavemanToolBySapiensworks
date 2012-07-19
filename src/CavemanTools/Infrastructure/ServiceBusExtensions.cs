using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CavemanTools.Infrastructure
{
    public static class ServiceBusExtensions
    {
        /// <summary>
        /// Scans assembly and registers all handlers found (commands and events)
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="asm"></param>
        /// <param name="resolver"></param>
        public static void RegisterHandlers(this ISetupServiceBus bus,Assembly asm,Func<Type,object> resolver)
        {
           foreach(var tp in asm.GetTypes())
           {
               var all = tp.GetInterfaces().Where(it => it.IsGenericType && it.GetGenericArguments()[0].Implements<IMessage>());
               if (all.Count() == 0) continue;
                foreach(var i in all)
                {
                    var paramType = i.GetGenericArguments()[0];
                    
                    var inst = resolver(tp);
                    bus.RegisterHandler(paramType, inst);
                }
               
           }
           
        }

        /// <summary>
        /// Tries to register provided type as a handler. The type must implement ICommandHandler or IEventHandler
        /// in order to be eligible.
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void RegisterHandlerFromInstance(this ISetupServiceBus bus,object t)
        {
            var all = t.GetType().GetInterfaces().Where(it => it.IsGenericType && it.GetGenericArguments()[0].Implements<IMessage>());

            if (all.Count() == 0) throw new ArgumentException("Specified argument is not a command or an event handler");
            

            foreach (var i in all)
            {
                var paramType = i.GetGenericArguments()[0];

                bus.RegisterHandler(paramType, t);
            }                        
        }

     

        /// <summary>
        /// Scans assembly of type and registers all command and events handlers found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="resolver"></param>
        public static void RegisterHandlersFromAssemblyOf<T>(this ISetupServiceBus bus,Func<Type,object> resolver)
        {
            RegisterHandlers(bus,Assembly.GetAssembly(typeof(T)),resolver);
        }
        
      
    }
}