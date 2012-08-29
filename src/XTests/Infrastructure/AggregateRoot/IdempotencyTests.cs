using System.Collections.Generic;
using System.Linq;
using CavemanTools.Infrastructure;
using CavemanTools.Infrastructure.MessagesBus;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Infrastructure.AggregateRoot
{
    public class FirstEvent:AbstractEvent
    {
        
    }

    public class MyAr:AggregateRoot<IEvent>
    {
        public MyAr(Guid id, IEnumerable<IEvent> events) : base(id, events)
        {
        }

        public MyAr(Guid id) : base(id)
        {
        }

        public void DoFirstEvent()
        {
            ApplyChange(new FirstEvent());
        }

        protected override void PlayEvent(IEvent ev)
        {
            Apply((dynamic)ev);
        }

        void Apply(FirstEvent ev)
        {
            
        }

        public static MyAr Create()
        {
            return new MyAr(Guid.NewGuid(),new[]
                                               {
                                                   new FirstEvent()
                                                       {
                                                           
                                                       }, 
                                               });
        }
    }
    public class IdempotencyTests
    {
        private Stopwatch _t = new Stopwatch();
        private MyAr _ar;

        public IdempotencyTests()
        {
            _ar = new MyAr(Guid.NewGuid());
        }

        [Fact]
        public void missing_operation_id_throws()
        {
            Assert.Throws<InvalidOperationException>(()=>_ar.DoFirstEvent());
        }

        [Fact]
        public void setting_an_already_used_operation_id_throws()
        {
            var opId = Guid.NewGuid();
            var ar=new MyAr(Guid.NewGuid(), new[]
                                               {
                                                   new FirstEvent()
                                                       {
                                                        SourceId   = opId
                                                       }, 
                                               });
            Assert.Throws<DuplicateOperationException>(()=>
                                                           {
                                                               ar.SetOperationId(opId);
                                                           });
            Assert.DoesNotThrow(()=>
                                    {
                                        ar.SetOperationId(Guid.NewGuid());
                                    });
        }

        [Fact]
        public void events_get_current_operation_id()
        {
            var opId = Guid.NewGuid();
            _ar.SetOperationId(opId);
            _ar.DoFirstEvent();
            var ch = _ar.GetChanges();
            Assert.Equal(1,ch.Count());
            var ev = ch.First();
            Assert.Equal(opId,ev.SourceId);
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}