namespace CavemanTools.Web.Security
{
    public class DefaultAuthorizationScopeId:AuthorizationScopeId
    {
        public DefaultAuthorizationScopeId(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }

        public override bool Equals(AuthorizationScopeId other)
        {
            var o = other as DefaultAuthorizationScopeId;
            if (o==null) return false;
            return o.Value == Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DefaultAuthorizationScopeId);
        }
    }
}