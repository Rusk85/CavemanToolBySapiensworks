using System;
using HtmlTags;

namespace MvcHtmlConventions
{
    public class LabelTag : HtmlTag
    {
        public LabelTag():base("label")
        {
            
        }

        public LabelTag(Action<HtmlTag> configure):base("label",configure)
        {
            
        }

        public LabelTag(HtmlTag parent):base("label",parent)
        {
            
        }

        public HtmlTag For(string tagId)
        {
            return this.Attr("for", tagId);
        }
    }
}