using System;
using System.Reflection;

namespace System.Web
{
    public static class Extensions
    {
        /// <summary>
        /// MyDll.Namespace.MyType -> ~/MyDll/Namespace/MyType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="excludeName">Exclude assembly name from namespace</param>
        /// <param name="removeControllerSuffix">If the type is a mvc controller</param>
        /// <returns></returns>
        public static string ToWebsiteRelativePath(this Type type,Assembly excludeName=null,bool removeControllerSuffix=true)
        {
            //var nspace=type.Namespace;
            //if (excludeName != null)
            //{
            //    var asmName = excludeName.GetName().Name;
            //    nspace = nspace.Substring(asmName.Length + 1);
            //}

            var nspace = Urlize(type.Namespace, excludeName);
            var result = "~/" + nspace+"/";
            if (removeControllerSuffix)
            {
                result += type.Name.Substring(0, type.Name.Length - 10);
            }
            else
            {
                result += type.Name;
            }
            return result;
        }

        /// <summary>
        /// Assembly.Namespace.Hello.WorldType -> [Assembly/]Namespace/Hello
        /// </summary>
        /// <param name="namespace"></param>
        /// <param name="excludeAssemblyName"></param>
        /// <returns></returns>
        public static string Urlize(this string @namespace, Assembly excludeAssemblyName =null)
        {
            var nspace = @namespace;
            if (excludeAssemblyName != null)
            {
                var asmName = excludeAssemblyName.GetName().Name;
                nspace = nspace.Substring(asmName.Length + 1);
            }
            return nspace.Replace('.', '/');
        }
    }
}