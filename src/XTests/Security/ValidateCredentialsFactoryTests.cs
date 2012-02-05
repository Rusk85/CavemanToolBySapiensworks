using CavemanTools.Web.Security.AccessRules;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    //public class ValidateCredentialsFactoryTests
    //{
    //    private Stopwatch _t = new Stopwatch();
    //    private ValidateCredentialsFactory _f;

    //    public ValidateCredentialsFactoryTests()
    //    {
    //        _f = new ValidateCredentialsFactory();
            
    //    }

    //    [Fact]
    //    public void standard_rule_registration_and_retrieval_()
    //    {
    //      _f.RegisterAccessRule<PublicAccess>();
    //        var rule = _f.GetAccessRule(typeof (PublicAccess));
    //        Assert.NotNull(rule);
    //        Assert.IsType<PublicAccess>(rule);
    //    }

    //    [Fact]
    //    public void requesting_unregistered_type_throws()
    //    {
    //        Assert.Throws<ArgumentException>(() =>
    //                                             {
    //                                                 _f.GetAccessRule<PublicAccess>();
    //                                             });
    //    }

    //    [Fact]
    //    public void requesting_by_typename_works_ok()
    //    {
    //        _f.RegisterAccessRule<PublicAccess>();
    //        var rule = _f.GetAccessRule(typeof (PublicAccess).FullName);
    //        Assert.NotNull(rule);
    //    }

    //    [Fact]
    //    public void registration_with_a_custom_factory_method_works_ok()
    //    {
    //        var ok=false;
    //        _f.RegisterAccessRule<PublicAccess>(t=>
    //                                                {
    //                                                    ok=true;
    //                                                    return new PublicAccess();
    //                                                });
    //        var rule = _f.GetAccessRule(typeof (PublicAccess).FullName);
    //        Assert.True(ok);
    //        Assert.NotNull(rule);
    //    }

    //    private void Write(string format, params object[] param)
    //    {
    //        Console.WriteLine(format, param);
    //    }
    //}
}