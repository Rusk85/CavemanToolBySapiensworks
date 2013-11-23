using HtmlTags;

namespace MvcHtmlConventions
{
    public class TextboxElement : IBuildElement
    {
        public bool AppliesTo(ModelInfo info)
        {
            return true;
        }

        public HtmlTag Build(ModelInfo info)
        {
            var tag = new TextboxTag();
            return tag;
        }
    }

    //public class SetNameAndValue
}