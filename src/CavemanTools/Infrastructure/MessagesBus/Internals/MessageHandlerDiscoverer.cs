using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal static class MessageHandlerDiscoverer
    {
        private const string CommandHandlerInterfaceName = "IExecuteCommand";
        private const string RequestHandlerInterfaceName = "IExecuteRequest";
        private const string EventHandlerInterfaceName = "ISubscribeToEvent";
        private const string StartsSagaInterfaceName = "ISagaStartedBy";
        private static string[] InterfaceNames = new[] {CommandHandlerInterfaceName,RequestHandlerInterfaceName,EventHandlerInterfaceName,StartsSagaInterfaceName};
      
        private static Type[] MessageTypes = new[] {typeof(ICommand),typeof(IEvent)};

        /// <summary>
        /// Returns the interfaces types usable by the message bus.
        /// </summary>
        /// <param name="handlerType"></param>
        /// <param name="msgType">Only interfaces handling this message type</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementedInterfaces(Type handlerType,Type msgType=null)
        {
            var rez= handlerType.GetInterfaces().Where(
                i => 
                i.IsGenericType && InterfaceNames.Any(n => i.Name.StartsWith(n)) &&
                MessageTypes.Any(m => i.GetGenericArguments()[0].Implements(m)));
            if (msgType!=null)
            {
                rez = rez.Where(i => i.GetGenericArguments()[0].Equals(msgType));
            }
            var list = rez.ToList();
            //filter ISubscribeToEvent when ISagaStartedBy is available
            var events =
                rez.Where(t => t.Name.StartsWith(StartsSagaInterfaceName)).Select(i => i.GetGenericArguments()[0]).ToArray();
            list.RemoveAll(t => t.Name.StartsWith(EventHandlerInterfaceName) && (events.Contains(t.GetGenericArguments()[0])));
            return list;

        }

        public static bool CanExecuteRequest(this Type[] interfaces)
        {
            return CanExecute(interfaces, RequestHandlerInterfaceName);
        }

        public static bool CanExecuteCommand(this Type[] interfaces)
        {
            return CanExecute(interfaces, CommandHandlerInterfaceName);
        }

        public static bool CanHandleEvent(this Type[] interfaces)
        {
            return CanExecute(interfaces, EventHandlerInterfaceName);
        }
    
        public static bool CanStartSaga(this Type[] interfaces)
        {
            return CanExecute(interfaces, StartsSagaInterfaceName);
        }

        private static bool CanExecute(Type[] interfaces, string name)
        {
            return interfaces.Any(i => i.Name.StartsWith(name));
        }

        
    }
}