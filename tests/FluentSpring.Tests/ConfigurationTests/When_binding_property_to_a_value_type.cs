using System;
using System.Collections.Generic;
using FluentSpring.Context;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_property_to_a_value_type : FluentContextTestBaseClass
    {
        [Test]
        public void Then_object_datetime_property_must_be_set_to_its_bound_value()
        {
            const string identifier = "myobjectname";
            DateTime propertyValue = DateTime.Now;

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.ADateTimeProperty)
                    .To(propertyValue);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.AreEqual(propertyValue, initialisedObject.ADateTimeProperty);
        }

        [Test]
        public void Then_object_dictionary_property_must_be_set_to_its_bound_value()
        {
            const string identifier = "myobjectname";
            IDictionary<string, string> propertyValue = new Dictionary<string, string>();
            propertyValue.Add("key", "value");

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.ADictionaryProperty).To(propertyValue);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.AreSame(propertyValue, initialisedObject.ADictionaryProperty);
            Assert.IsTrue(initialisedObject.ADictionaryProperty.ContainsKey("key"));
            Assert.AreEqual("value", initialisedObject.ADictionaryProperty["key"]);
        }

        [Test]
        public void Then_the_object_int_property_must_be_set_to_its_bound_value()
        {
            const string identifier = "myobjectname";
            const int propertyValue = 10;

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.AIntegerProperty).To(propertyValue);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.AreEqual(propertyValue, initialisedObject.AIntegerProperty);
        }

        [Test]
        public void Then_the_object_string_property_must_be_set_to_its_bound_value()
        {
            const string identifier = "myobjectname";
            const string propertyValue = "someValue";

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.AStringProperty).To(propertyValue);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.AreEqual(propertyValue, initialisedObject.AStringProperty);
        }
    }
}