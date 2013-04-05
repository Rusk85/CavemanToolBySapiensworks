﻿using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Routing
{
    public static class Extensions
    {
         public static string StripNamespaceRoot(this RoutingPolicySettings settings, string @namespace)
         {
             return @namespace.Remove(0, settings.NamespaceRoot.Length);
         }

        /// <summary>
        /// Register types deriving from Controller class
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        public static void RegisterControllers(this RoutingPolicy policy,Assembly asm)
        {
            Register(policy,asm,t =>t.DerivesFrom<Controller>());          
        }
        /// <summary>
        /// Register as actions types matching a criteria
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="asm"></param>
        /// <param name="match"></param>
        public static void Register(this RoutingPolicy policy, Assembly asm, Func<Type, bool> match)
        {
            RegisterActions(policy, asm.GetTypes().Where(match).ToArray());           
        }

        /// <summary>
        /// Registers actions from controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="policy"></param>
        public static void Register<T>(this RoutingPolicy policy) where T : Controller
        {
            policy.RegisterActions(typeof(T));
        }

        /// <summary>
        /// Registers actions from the provided types.
        /// All types should be Controllers
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="controllers"></param>
        public static void RegisterActions(this RoutingPolicy policy, params Type[] controllers)
        {
            foreach (var c in controllers)
            {
                c.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).ForEach(m =>
                {
                    policy.AddAction(new ActionCall(m, policy.Settings));
                }); 
            }           
        }
    }
}