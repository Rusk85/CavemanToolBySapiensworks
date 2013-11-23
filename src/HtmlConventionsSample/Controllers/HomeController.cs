using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlConventionsSample.Models;
using Ploeh.AutoFixture;

namespace HtmlConventionsSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var f = new Fixture();
            return View(f.Create<MyModel>());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}