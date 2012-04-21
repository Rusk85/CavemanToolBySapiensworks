namespace CavemanTools.Web.Security
{
    public class DefaultAuthorizationScopeId:AuthorizationScopeId
    {
        public DefaultAuthorizationScopeId(int value)
        {
            Value = value;
        }

        public override bool Equals(AuthorizationScopeId other)
        {
            var o = other as DefaultAuthorizationScopeId;
            if (o==null) return false;
            return (int)o.Value == (int)Value;
        }

        public override object Value
        {
            get; protected set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DefaultAuthorizationScopeId);
        }
    }
}