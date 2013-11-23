using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HtmlTags;
using HtmlTags.Extended.Attributes;
using MvcHtmlConventions;

namespace HtmlConventionsSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Conventions();
        }

        void Conventions()
        {
            var editors = HtmlConventions.Instance.Editors;
            editors.DefaultBuilder(HtmlConventions.InputTagBuilder);
            editors.If(m =>
            {
                return m.RawValue.Is<bool>();
            })
                .Modify((tag, info) =>
                {
                    var input = tag.GetChild<TextboxTag>();
                    tag.Children.Remove(input);
                    tag.Children.Insert(0, new CheckboxTag(info.Value<bool>()).Name(info.HtmlName).Id(info.HtmlId));
                    return tag;
                });
            
            editors
                .If(m => !m.RawValue.Is<bool>() && !m.Type.IsUserDefinedClass())
                .Modify((tag, info) =>
                {
                    tag.GetChild<LabelTag>().AddClass("block");
                    return tag;
                });

            editors.Always.Modify((tag, i) =>
            {
                var wrapper = new DivTag();
                wrapper.AddClass("form-field");
                wrapper.Children.Add(tag);
                return wrapper;
            });

        }
    }
}
