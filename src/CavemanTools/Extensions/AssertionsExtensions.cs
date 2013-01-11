﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static void MustNotBeEmpty<T>(this IEnumerable<T> list,string paramName=null)
        {
            if (list.IsNullOrEmpty()) throw new ArgumentException("The collection must contain at least one element",paramName??"");
        }
            
        public static void MustMatch(this string source,string regex,RegexOptions options=RegexOptions.None)
        {
            if (source.IsNullOrEmpty() || !Regex.IsMatch(source,regex,options)) throw new FormatException(string.Format("Argument doesn't match expression '{0}'",regex));
        }

        /// <summary>
        /// Value type must be of specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static void MustBe<T>(this object value)
        {
            var tp = typeof (T);
            bool ex = false;
            if (value == null)
            {
                if (tp.IsClass) return;
                ex = true;
            }
            if (ex || (value.GetType()!=tp)) throw new ArgumentException("Argument must be of type '{0}'".ToFormat(tp));
        }

        /// <summary>
        /// Arugment must implement interface T
        /// </summary>
        /// <typeparam name="T">Inerface type</typeparam>
        /// <param name="value"></param>
        public static void MustImplement<T>(this object value)
        {
            value.MustNotBeNull("value");
            var tp = typeof (T);
            if (!tp.IsInterface) throw new ArgumentException("'{0}' is not an interface".ToFormat(tp));
            var otype = value.GetType();
            
            if (value is Type)
            {
                otype= value as Type;
            }

            if (!otype.Implements(tp)) throw new ArgumentException("Argument must implement '{0}'".ToFormat(tp));
        }

        /// <summary>
        /// List must not be empty and must have non-null values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="throwWhenNullValues"></param>
        public static void MustHaveValues<T>(this IEnumerable<T> list,bool throwWhenNullValues=true) where T : class
        {
            list.MustNotBeEmpty();
            
            if (throwWhenNullValues)
            {
                if (list.Any(v => v == null))
                {
                    throw new ArgumentException("The collection is null, empty or it contains null values");
                }
            }            
        }
    }
}