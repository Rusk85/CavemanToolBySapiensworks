using System.Collections;
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

        public static void MustNotBeEmpty(this ICollection list,string paramName=null)
        {
            if (list==null || list.Count==0) throw new ArgumentException("The collection must contain at least one element",paramName??"");
        }
            
        public static void MustMatch(this string source,string regex,RegexOptions options=RegexOptions.None)
        {
            if (source.IsNullOrEmpty() || !Regex.IsMatch(source,regex,options)) throw new FormatException(string.Format("Argument doesn't match expression '{0}'",regex));
        }
    }
}