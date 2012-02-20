using System.Web;
using CavemanTools.Extensions;
using Xunit;
using CavemanTools.Web;

namespace XTests.Cookies
{
	public class ExtensionTests
	{
		private HttpCookie _ck;

		public ExtensionTests()
		{
			_ck = new HttpCookie("test");
		}

		[Fact]
		public void Get_Typed_Object_From_Cookie()
		{
			_ck.Value = "5";
			Assert.Equal(5,_ck.GetValue<int>());
				
		}

		[Fact]
		public void Invalid_Cookie_Returns_Default()
		{
			_ck.Value = "fff";
			Assert.Equal(6, _ck.GetValue(6));
		}

		[Fact]
		public void Cookie_Value_overrides_provided_default()
		{
			_ck.Value = "45";
			Assert.Equal(45, _ck.GetValue(6));
		}
	}
}