using HtmlTags;

namespace MvcHtmlConventions
{
    public class ValidationFieldTag : HtmlTag
    {
        public ValidationFieldTag(string tagId):base("span")
        {
            AddClass("field-validation-valid")
                .Data("valmsg-for", tagId)
                .Data("valmsg-replace", "true");
        }
    }
}