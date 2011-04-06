using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;
using Is = FluentSpring.Context.Configuration.Constraints.Is;

namespace FluentSpring.Tests.Demo
{
    [TestFixture]
    public class Fluent_spring_configuration_examples
    {
        public void Demo()
        {
            FluentApplicationContext.Register<TestObject>()
                .AsNonSingleton()
                .Bind(x => x.PropertyX).To("a value");


            FluentApplicationContext.Register<TestObject>().Bind(x => x.PropertyX).To<String>();

            FluentApplicationContext.Register<TestObject>().Bind(x => x.PropertyX).To<String>("something");

            FluentApplicationContext.Register<TestObject>().Bind(x => x.PropertyX).To("something");

            FluentApplicationContext.Register<TestObject>().Bind(x => x.SomeDictionary).ToDefinition(
                Inline.Dictionary<string, String>()
                    .AddEntry().WithKey<String>().AndValue("adsf")
                    .AddEntry().WithKey("asdfasdf").AndValue<String>()
                    .AddEntry().WithKey("asdfasdf").AndValueDefinition(
                        Inline.Object<String>()
                            .Bind(x => x.ToString()).To("1"))
                    .AddEntry()
                    .WithKeyDefinition(
                        Inline.Object<string>())
                    .AndValueDefinition(
                        Inline.Object<String>()
                            .Bind(x => x.ToString()).To("1"))
                );

            Inline.Dictionary<string, string>()
                .AddEntry().WithKey<String>().AndValue("adsf")
                .AddEntry().WithKey("asdfasdf").AndValue<String>();

            FluentApplicationContext.Register<TestObject>()
                .BindConstructorArgument<String>()
                    .ToDefinition(Inline.Object<String>());

            FluentApplicationContext.Bind<IDictionary<string, string>>()
                .To<Dictionary<string, string>>().When(Is.RunningEnvironment("PROD"));
        }

        [Test]
        public void Expression()
        {
            MyClass c = new MyClass();
            //c.Register<TestObject>(() => new TestObject());

            c.Register<ObjectWithConstructor>(context => new ObjectWithConstructor(context.GetObject<TestObject>(),"some string")
            {
                TestObject = context.GetObject<TestObject>()
            });
            c.Register<ObjectWithConstructor>(() => new ObjectWithConstructor(new ObjectWithProperties())
            {
                TestObject = new TestObject()
            });
        }
    }

    public class MyClass
    {
        public void Register<T>(Expression<Func<IApplicationContext, T>> myexp)
        {
            string s = myexp.ToString();
        }

        public void Register<T>(Expression<Func<T>> myexp)
        {
            string s = "1";
        }
    }
}