using System.Web.Mvc;
using CavemanTools.Mvc.Controllers;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Mvc.Controllers
{
    
    public class TestModel
    {
        public string Data { get; set; }
        public int Other { get; set; }
    }

    public class MyController:SmartController
    {
        public MyController()
        {
           // ActionSuccessResult = () => Content("success");
            SetupModel<TestModel>(tm=>tm.Other=23);
        }

        public ActionResult Add()
        {
            return View(PopulateModel(new TestModel()));
        }

        //[HttpPost]
        //public ActionResult AddFail(TestModel model)
        //{
        //    return UpdateModel(model, m =>
        //                                        {
        //                                            ModelState.AddModelError("data", "empty");
        //                                        });
        //}

        //[HttpPost]
        //public ActionResult AddFailInlineHandler(TestModel model)
        //{
        //    return UpdateModel(model, m =>
        //    {
        //        ModelState.AddModelError("data", "empty");
        //    },failure:JsonResultError(model));
        //}

        //[HttpPost]
        //public ActionResult Add(TestModel model)
        //{
        //    return UpdateModel(model, m =>
        //                                        {
                                                    
        //                                        });
        //}
     
    }

    public class BaseControllerTests
    {
        private Stopwatch _t = new Stopwatch();
        private MyController _c;

        public BaseControllerTests()
        {
            _c = new MyController();
        }

        [Fact]
        public void add_action_returns_testmodel()
        {
            var rez = _c.Add() as ViewResult;
            var model = rez.Model as TestModel;
            Assert.NotNull(model);
            Assert.Equal(23,model.Other);
        }

        //[Fact]
        //public void add_action_post_successful()
        //{
        //    var model = new TestModel() {Data = "input"};
        //    var rez = _c.Add(model) as ContentResult;
        //    Assert.Equal("success",rez.Content);
        //}

        //[Fact]
        //public void add_action_post_fails()
        //{
        //    var model = new TestModel() { Data = "input",Other = 0};
        //    var rez = _c.AddFail(model) as ViewResult;
        //   var vm = rez.Model as TestModel;
        //    Assert.False(rez.ViewData.ModelState.IsValid);
        //    Assert.Equal("input",vm.Data);
        //    Assert.Equal(23,vm.Other);
        //}


        //[Fact]
        //public void add_action_post_fails_inline_handler()
        //{
        //    var model = new TestModel() { Data = "input", Other = 0 };
        //    var rez = _c.AddFailInlineHandler(model) as JsonResult;
        //    Assert.NotNull(rez);            
        //}

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}