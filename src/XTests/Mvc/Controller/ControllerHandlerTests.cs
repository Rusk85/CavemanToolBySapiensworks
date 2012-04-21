using System.Web.Mvc;
using CavemanTools.Model;
using CavemanTools.Mvc.UnitTesting;
using Xunit;
using Moq;
using System;
using System.Diagnostics;

namespace XTests.Mvc.Controller
{
    public class ControllerHandlerTests
    {
        public class Command
        {
            public int Id { get; set; }
            public bool Handled { get; set; }
        }

        class CmdHandler:IHandleCommand<Command>
        {
            
            public void Handle(Command command)
            {
                if (command.Id==0) throw new Exception();
                if (command.Id==7)
                {
                    throw new BusinessRuleException("test","hello");
                }
                command.Handled = true;
            }
        }

        private Stopwatch _t = new Stopwatch();
      
        private Mock<IDependencyResolver> _ioc;
      
        private FakeController _ctrl;


        public ControllerHandlerTests()
        {
            _ctrl = new FakeController();
            _ioc = new Mock<IDependencyResolver>();
            
            _ioc.Setup(d => d.GetService(typeof(IHandleCommand<Command>))).Returns(new CmdHandler());
           
        }

        [Fact]
        public void handle_command()
        {
            var c = new Command() { Id = 3 };
            ControllerExtensions.Handle(_ctrl, c, new CmdHandler());
            Assert.True(c.Handled);
        }

        [Fact]
        public void handle_command_with_ioc()
        {
            var c = new Command() {Id = 3};
            ControllerExtensions.Handle(_ctrl,c,_ioc.Object);
            Assert.True(c.Handled);
        }


        [Fact]
        public void errors_exception_are_copied_to_modelstate()
        {
            var c = new Command() {Id = 7};
            Assert.DoesNotThrow(()=>ControllerExtensions.Handle(_ctrl,c,_ioc.Object));
            Assert.True(_ctrl.ModelState.ContainsKey("test"));
        }

        [Fact]
        public void any_non_validate_errors_exception_is_let_through()
        {
            Assert.Throws<Exception>(() => _ctrl.Handle(new Command(), _ioc.Object));
        }

        [Fact]
        public void process_command_with_lambda()
        {
            Assert.Equal(0,_ctrl.Process(new Command(), c => 0));
        }

        [Fact]
        public void null_handler_throws()
        {
            Func<Command, int> f = null;
            Assert.Throws<ArgumentNullException>(() => _ctrl.Process(new Command(),f));
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}