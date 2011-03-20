using System;
using System.Collections.Generic;
using FluentSpring.Context;
using FluentSpring.Context.Configuration.Constraints;
using FluentSpring.Tests.TestObjects;

namespace FluentSpring.Tests.Demo
{
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
    }
}