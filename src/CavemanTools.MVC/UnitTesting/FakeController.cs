using System.Web.Mvc;

namespace CavemanTools.Mvc.UnitTesting
{
    public class FakeController:Controller
    {
        public FakeController()
        {
            ViewData= new ViewDataDictionary();
        }  
    }
}