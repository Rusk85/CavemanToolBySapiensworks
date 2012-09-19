﻿using System.Collections;

namespace System
{
    public static class AssertionsExtensions
    {
         public static void MustNotBeNull<T>(this T param,string paramName=null) where T:class
         {
             if (param == null) throw new ArgumentNullException(paramName??string.Empty);
         }

        public static void MustNotBeEmpty(this string arg,string paramName=null)
        {
            if (string.IsNullOrWhiteSpace(arg)) throw new FormatException(string.Format("Argument '{0}' must not be null, empty or whitespaces",paramName??""));
        }

        public static void MustNotBeEmpty(this ICollection list,string paramName=null)
        {
            if (list==null || list.Count==0) throw new ArgumentException("The collection must contain at least one element",paramName??"");
        }
            
    }
}