using Xunit;
using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;

namespace XTests.Extensions
{
    public class TypeExtensiont
    {
        private Stopwatch _t = new Stopwatch();

        public TypeExtensiont()
        {

        }

        [Fact]
        public void user_defined_reference_types()
        {
            var objects = new object[] {2,Guid.Empty,new byte[0], EnvironmentVariableTarget.Process,"string"};
            
            objects.ForEach(o=>o.GetType().IsUserDefinedClass().Should().BeFalse("{0}".ToFormat(o)));

            typeof (int?).IsUserDefinedClass().Should().BeFalse();

            this.GetType().IsUserDefinedClass().Should().BeTrue();
        }

        public interface ISaveSaga<T>
        {
            void Save(T data);
        }

        public class SaveSaga : ISaveSaga<string>
        {
            public void Save(string data)
            {
                throw new NotImplementedException();
            }
        }

        public abstract class MyClass<T>
        {
             public interface IInternal<V>
             {
                 T Bla(V arg);
             }
        }

        public class Myimpl:MyClass<string>.IInternal<int>
        {
            public string Bla(int arg)
            {
                throw new NotImplementedException();
            }
        }

        public class MyImpl2:MyClass<string>
        {
            
        }

        [Fact]
        public void type_implements_generic_interface_no_args()
        {
            var t = typeof (SaveSaga);
            t.ImplementsGenericInterface("ISaveSaga").Should().BeTrue();

            t = typeof (Myimpl);
            t.ImplementsGenericInterface("IInternal").Should().BeTrue();
            
        }

        [Fact]
        public void type_implements_generic_interface_with_args()
        {
            var t = typeof (Myimpl);
            typeof (SaveSaga).ImplementsGenericInterface("ISaveSaga", typeof (string)).Should().BeTrue();
            t.ImplementsGenericInterface("IInternal",typeof(string), typeof(int)).Should().BeTrue();            
        }

        [Fact]
        public void type_extends_generic_class_with_args()
        {
            var t = typeof(MyImpl2);
            t.ExtendsGenericType("MyClass", typeof (string)).Should().BeTrue();
        }

        protected void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}