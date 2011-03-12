using System;
using System.Linq;
using FluentSpring.Context;
using FluentSpring.Context.Configuration.Conventions;
using FluentSpring.Context.Configuration.Conventions.Support;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;
using FluentSpring.Context.Extensions;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_adding_a_convention : FluentContextTestBaseClass
    {
        [Description("Demo")]
        public void Then_my_object_configuration_must_apply_the_convention_I_created()
        {
            //ITypeFilterLookup assemblyTypeFilterLookup = new TypeFilterConvention();
            

            //ITypeNameLookup typeNameConvention = new TypeNameConvention(t => t.FullName);
            //PropertyBindingConvention propertyBindingConvention = new PropertyBindingConvention(x => typeNameConvention.GetTypeNameFor(x.DeclaringType));

            //FluentApplicationContext
            //    .Apply(() => propertyBindingConvention)
            //    .Apply(() => typeNameConvention)
            //    .To(assemblyTypeFilterLookup.GetAllTypes);


            //FluentApplicationContext.RegisterAll(assemblyTypeFilterLookup.GetAllTypes);

            FluentApplicationContext.In(()=>AppDomain.CurrentDomain.GetAssemblies())
        }

        //[Test]
        //public void Then_the_type_filter_should_only_return_the_selected_types()
        //{
        //    ITypeFilterLookup assemblyTypeConvention = new TypeFilterConvention();
        //    assemblyTypeConvention
        //        .For(t=>t.FullName.Equals(typeof(When_adding_a_convention).FullName))
        //        .In(a=>a.FullName.Contains("FluentSpring"));

        //    IList<Type> types = assemblyTypeConvention.GetAllTypes();

        //    Assert.AreEqual(1, types.Count);
        //}

        //[Test]
        //public void Then_the_type_name_convention_must_apply_the_convention_to_the_type()
        //{
        //    IConvention typeNameConvention = new TypeNameConvention(t => t.Name);

        //    string computedName = typeNameConvention.GetTypeNameFor(typeof (When_adding_a_convention));

        //    Assert.AreEqual(typeof(When_adding_a_convention).Name, computedName);
        //}

        //[Test]
        //public void Then_the_property_binding_convention_must_return_the_type_name_to_inject()
        //{
        //    ITypeNameLookup typeNameConvention = new TypeNameConvention(t => t.Name);
        //    IInjectedPropertyLookup propertyBindingConvention = new PropertyBindingConvention(x => typeNameConvention.GetTypeNameFor(x.DeclaringType));

        //    foreach (var propertyInfo in typeof(TestObject).GetProperties())
        //    {
        //        string computedTypeName = propertyBindingConvention.GetInjectedTypeFor(propertyInfo);
        //        Assert.AreEqual(computedTypeName, propertyInfo.DeclaringType.Name);
        //    }
        //}

        //[Test]
        //public void Then_the_registered_object_must_apply_the_type_name_convention()
        //{
        //    TypeNameConvention typeNameConvention = new TypeNameConvention(t => t.Name);

        //    FluentApplicationContext.Register<TestObject>()
        //        .Bind(x => x.PropertyX).To("X")
        //        .Apply(typeNameConvention);

        //    IApplicationContext context = _applicationContextContainer.InitialiseContext();

        //    var testObject = context.GetObject<TestObject>(typeof(TestObject).Name);

        //    Assert.IsNotNull(testObject);
        //}

        //[Test]
        //public void Then_the_registered_object_must_apply_the_property_binding_convention()
        //{
        //    PropertyBindingConvention bindingConvention = new PropertyBindingConvention();
        //}
    }
}
