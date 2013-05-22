namespace CavemanTools.Web.Authentication
{
    /// <summary>
    /// Uses Int32 as id type
    /// </summary>
    public class DefaultAuthorizationScopeId:AuthorizationScopeId
    {
        public DefaultAuthorizationScopeId(int value)
        {
            Value = value;
        }

        public override bool Equals(AuthorizationScopeId other)
        {
            if (other == null) return false;
            var o = other as DefaultAuthorizationScopeId;
            if (o==null) return false;
            return (int)o.Value == (int)Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DefaultAuthorizationScopeId);
        }
    }
}