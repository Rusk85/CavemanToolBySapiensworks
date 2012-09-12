using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    internal class TypeWithHandlers
    {
        private readonly Type _handlerType;
        private readonly IEnumerable<Type> _messagesTypes;

        public TypeWithHandlers(Type handlerType,IEnumerable<Type> messagesTypes)
        {
            _handlerType = handlerType;
            _messagesTypes = messagesTypes;
        }

        public IEnumerable<Type> MessagesTypes
        {
            get { return _messagesTypes; }
        }

        public Type HandlerTypeName
        {
            get { return _handlerType; }
        }

        public static TypeWithHandlers TryCreateFrom(Type t)
        {
            var alli = MessageHandlerDiscoverer.GetImplementedInterfaces(t).Select(i => i.GetGenericArguments()[0]).ToArray();
                //t.GetInterfaces().Where(
                //    i =>
                //    i.IsGenericType && MessageHandlerDiscoverer.InterfaceNames.Any(n => i.Name.StartsWith(n)) &&
                //    MessageHandlerDiscoverer.MessageTypes.Any(m => i.GetGenericArguments()[0].Implements(m)))
                //    .Select(i=> i.GetGenericArguments()[0])
                //    .ToArray();
            if (alli.Length==0)
            {
                return null;
            }
            return new TypeWithHandlers(t, alli);
        }

    }
}