using System;
using System.Linq;
using System.Reflection;

namespace CavemanTools.Infrastructure
{
    public static class ServiceBusExtensions
    {
        /// <summary>
        /// Scans assembly and registers all command handlers found
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="asm"></param>
        /// <param name="resolver"></param>
        public static void RegisterHandlers(this ISetupServiceBus bus,Assembly asm,Func<Type,object> resolver)
        {
            MethodInfo mi=null;
            foreach(var m in bus.GetType().GetMethods().Where(d=>d.Name=="RegisterCommandHandlerFor"))
            {
                var p = m.GetParameters()[0];
                if (p.ParameterType.IsInterface)
                {
                    mi = m;
                    break;
                }
            }
            if (mi==null)
            {
                throw new Exception("Something is wrong, probably a bug");
            }
            
            foreach(var tp in asm.GetTypes())
            {
                var all=tp.GetInterfaces().Where(it=>it.IsGenericType && it.GetGenericArguments()[0].Implements<ICommand>());
                
                if (all.Count()==0) continue;
                foreach(var i in all)
                {
                    var paramType = i.GetGenericArguments()[0];
                    
                    var inst = resolver(tp);
                    var mm=mi.MakeGenericMethod(paramType);
                    mm.Invoke(bus, new object[] { inst });
                }
                
                
            }
            
        }

        /// <summary>
        /// Scans assembly and registers all event handlers found
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="asm"></param>
        /// <param name="resolver"></param>
        public static void RegisterSubscribers(this ISetupServiceBus bus, Assembly asm, Func<Type, object> resolver)
        {
            MethodInfo mi = null;
            foreach (var m in bus.GetType().GetMethods().Where(d => d.Name == "SubscribeToEvent"))
            {
                var p = m.GetParameters()[0];
                if (p.ParameterType.IsInterface)
                {
                    mi = m;
                    break;
                }
            }
            if (mi == null)
            {
                throw new MissingMethodException("ServiceBus method not found");
            }

            foreach (var tp in asm.GetTypes())
            {
                var all = tp.GetInterfaces().Where(it => it.IsGenericType && it.GetGenericArguments()[0].Implements<IEvent>());

                if (all.Count() == 0) continue;
                foreach (var i in all)
                {
                    var paramType = i.GetGenericArguments()[0];

                    var inst = resolver(tp);
                    var mm = mi.MakeGenericMethod(paramType);
                    mm.Invoke(bus, new object[] { inst });
                }


            }

        }

        /// <summary>
        /// Scans assembly of type and registers all command handlers found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="resolver"></param>
        public static void RegisterHandlersFromAssemblyOf<T>(this ISetupServiceBus bus,Func<Type,object> resolver)
        {
            RegisterHandlers(bus,Assembly.GetAssembly(typeof(T)),resolver);
        }
        
        /// <summary>
        /// Scans assembly of type and registers all event handlers found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="resolver"></param>
        public static void RegisterSubscribersFromAssemblyOf<T>(this ISetupServiceBus bus,Func<Type,object> resolver)
        {
            RegisterSubscribers(bus,Assembly.GetAssembly(typeof(T)),resolver);
        }
    }
}