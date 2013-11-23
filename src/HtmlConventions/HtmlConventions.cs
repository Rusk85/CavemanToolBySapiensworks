using HtmlTags;
using MvcHtmlConventions.Internals;

namespace MvcHtmlConventions
{
    public class HtmlConventions
    {
        static HtmlConventions instance=new HtmlConventions();
        public static HtmlConventions Instance
        {
            get
            {
                return instance;
            }
        }
          
        public HtmlConventions()
        {
           Editors=new ConventionsRegistry();  
        }

        public IDefinedConventions Editors { get; private set; }


        /// <summary>
        /// Default type is text
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static HtmlTag InputTagBuilder(ModelInfo info)
        {
            var tag = HtmlTag.Placeholder();
            var label = new LabelTag().Attr("for", info.HtmlId).Text(info.Name);
            tag.Children.Add(label);
            tag.Children.Add(new TextboxTag(info.HtmlName, info.RawValue.ToString()).Id(info.HtmlId));
            tag.Children.Add(new ValidationFieldTag(info.HtmlId));
            return tag;
        }

        
    }
}