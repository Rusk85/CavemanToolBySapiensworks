using System.Reflection;

namespace System.Linq.Expressions
{
    public static  class ExpressionExtensions
	{
		/// <summary>
		/// Returns reflection information for a property expression
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="property">Lambda returning the property</param>
		/// <returns></returns>
		public static MemberInfo GetPropertyInfo<T>(this Expression<Func<T,object>> property)
		{
			if (property == null) throw new ArgumentNullException("property");
			var prop = property.Body as MemberExpression;
			if (prop == null && property.Body is UnaryExpression)
			{
				prop = (property.Body as UnaryExpression).Operand as MemberExpression;
			}
			if (prop == null) throw new FormatException("Lambda should be a property");
			return prop.Member;
		}

      
        /// <summary>
        /// Returns reflection information for a method call expression
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="method">Lambda with method call</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T>(this Expression<Action<T>> method)
        {
            if (method == null) throw new ArgumentNullException("method");
            var prop = method.Body as MethodCallExpression;
            if (prop == null) throw new FormatException("Lambda should be a method call");
            return prop.Method;
        }


        public static bool HasParameterArgument(this MethodCallExpression node,Type type=null)
        {
            node.MustNotBeNull();
            foreach (var arg in node.Arguments)
            {
                if (arg.IsParameter(type) || arg.BelongsToParameter(type)) return true;
            }
            return false;
        }


        public static bool HasParameterArgument(this NewExpression node, Type type = null)
        {
            node.MustNotBeNull();
            foreach (var arg in node.Arguments)
            {
                if (arg.IsParameter(type) || arg.BelongsToParameter(type)) return true;
            }
            return false;
        }


        /// <summary>
        /// True if expresion belongs to a parameter or is a method which has a parameter argument.
        /// Doesn't handle lambdas
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool InvolvesParameter(this Expression node, Type type = null)
        {
            node.MustNotBeNull();
            if (node.IsParameter(type) || node.BelongsToParameter(type)) return true;
            if (node.NodeType == ExpressionType.Call)
            {
                return node.As<MethodCallExpression>().HasParameterArgument(type);
            }
            if (node.NodeType == ExpressionType.New)
            {
                return node.As<NewExpression>().HasParameterArgument(type);
            }

            return false;
        }

        /// <summary>
        /// True if expression belongs to a lambda argument (expression parameter)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool BelongsToParameter(this Expression node, Type type = null)
        {
            node.MustNotBeNull();
            Expression parent = null;

            switch (node.NodeType)
            {
                case ExpressionType.MemberAccess:
                    parent= node.As<MemberExpression>().Expression;
                    break;
                case ExpressionType.Call:
                    var m = node.As<MethodCallExpression>();
                    parent= m.Object;
                    if (m.HasParameterArgument(type))
                    {
                        return true;
                    }
                    break;
                case ExpressionType.Not:
                case ExpressionType.Convert:
                    var u = node as UnaryExpression;
                    parent = u.Operand;
                    break;
            }

            if (parent == null) return false;

            if (parent.NodeType != ExpressionType.Parameter)
            {
                return parent.BelongsToParameter(type);
            }



            if (type != null)
            {
                if (parent.Type != type) return false;
            }
            return true;
        }
      



        /// <summary>
        /// True if expression is a lambda argument 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="type">Argument type</param>
        /// <returns></returns>
        public static bool IsParameter(this Expression ex,Type type=null)
        {
            ex.MustNotBeNull();
            if (ex.NodeType != ExpressionType.Parameter) return false;
            if (type == null) return true;
            var par = ex as ParameterExpression;
            return (par.Type == type) ;
        }

        /// <summary>
        /// Gets the value of an expresion if it's a property,field,constant or method call
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object GetValue(this Expression node)
        {
            node.MustNotBeNull();
            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return node.As<ConstantExpression>().Value;                    
                case ExpressionType.New:
                    return node.As<NewExpression>().CreateObject();                    
                case ExpressionType.NewArrayInit:
                    return node.As<NewArrayExpression>().CreateArray();
                case ExpressionType.MemberAccess:
                    return node.As<MemberExpression>().GetValue();
                case ExpressionType.ArrayIndex:
                    return GetArrayIndex(node.As<BinaryExpression>());
                case ExpressionType.Call:
                    return node.As<MethodCallExpression>().GetValue();
            }
            throw new InvalidOperationException("You can get the value of a property,field,constant or method call");
        }

        internal static object GetArrayIndex(BinaryExpression node)
        {
            node.MustNotBeNull();
            if (node.NodeType != ExpressionType.ArrayIndex)
            {
                throw new NotSupportedException("Only array indexes are supported");
            }
            var arr = node.Left.GetValue().As<Array>();
            var idx = node.Right.GetValue();
            if (idx is int)
                return arr.GetValue((int) idx);
            if (idx is long)
            {
                return arr.GetValue((long) idx);
            }
            throw new NotSupportedException();
        }

        public static object CreateArray(this NewArrayExpression node)
        {
            node.MustNotBeNull();
            var args = node.Expressions.Select(a => a.GetValue()).ToArray();
            var arr = Array.CreateInstance(node.Type.GetElementType(), args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                arr.SetValue(args[i],i);                
            }
            return arr;
        }

        public static object CreateObject(this NewExpression node)
        {
            node.MustNotBeNull();
            var args = node.Arguments.Select(a => a.GetValue()).ToArray();
            if (node.Constructor == null)
            {
                return Activator.CreateInstance(node.Type, args);
            }
            return node.Constructor.Invoke(args);
        }

        /// <summary>
        /// Gets the value of the member access expression.
        /// Works with properties, fields
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object GetValue(this MemberExpression node)
        {
            object parentValue=null;

            if (node.Expression != null) //not static
            {
                if (node.Expression.CanReturnValue())
                {
                    parentValue = node.Expression.GetValue();
                }
            }

            if (node.Member.MemberType == MemberTypes.Property)
            {
                return node.Member.As<PropertyInfo>().GetValue(parentValue, null);
            }

            if (node.Member.MemberType == MemberTypes.Field)
            {
                return node.Member.As<FieldInfo>().GetValue(parentValue);
            }
           
            throw new InvalidOperationException();
        }

        /// <summary>
        /// True if the expression is of the type that can return a value (property,field,cosntnat, method call)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool CanReturnValue(this Expression node)
        {
            if (node == null) return false;
            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                case ExpressionType.New:
                case ExpressionType.NewArrayInit:
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.ArrayIndex:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object GetValue(this MethodCallExpression node)
        {
            node.MustNotBeNull();
            if (node.Arguments.Any(a => !a.CanReturnValue()))
            {
                throw new NotSupportedException("Can't identify the value of at least one argument");
            }
            var args = node.Arguments.Select(a => a.GetValue()).ToArray();
            object parent = null;
            
            if (node.Object != null && node.Object.CanReturnValue())
            {
                parent = node.Object.GetValue();
            }

            return node.Method.Invoke(parent, args);

         
        }
	}
}