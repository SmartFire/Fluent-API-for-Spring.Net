using System.Linq;
using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_binding_a_property_to_dictionary : FluentContextTestBaseClass
    {
        [Test]
        public void Then_the_dictionary_key_and_value_must_be_injected_with_inline_objects()
        {
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectKeyValueDictionary).ToDefinition(
                    Inline.Dictionary<TestObject, TestObject>()
                        .AddEntry()
                        .WithKeyDefinition(
                            Inline.Object<TestObject>()
                                .Bind(x => x.PropertyX).To("somekeyvalue")
                        )
                        .AndValueDefinition(
                            Inline.Object<TestObject>()
                                .Bind(x => x.PropertyX).To("somevalue"))
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectKeyValueDictionary.Keys.Where(l => l.PropertyX.Equals("somekeyvalue")).Count() > 0);
            Assert.IsTrue(initialisedObject.TestObjectKeyValueDictionary.Values.Where(l => l.PropertyX.Equals("somevalue")).Count() > 0);
            Assert.AreEqual("somevalue", initialisedObject.TestObjectKeyValueDictionary.Where(k => k.Value.PropertyX.Equals("somevalue")).Single().Value.PropertyX);
        }

        [Test]
        public void Then_the_dictionary_key_must_be_injected_with_inline_object()
        {
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectKeyDictionary).ToDefinition(
                    Inline.Dictionary<TestObject, string>()
                        .AddEntry()
                        .WithKeyDefinition(
                            Inline.Object<TestObject>()
                                .Bind(x => x.PropertyX).To("somevalue"))
                        .AndValue("blah"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectKeyDictionary.Keys.Where(l => l.PropertyX.Equals("somevalue")).Count() > 0);
            Assert.AreEqual("blah", initialisedObject.TestObjectKeyDictionary.Where(k => k.Key.PropertyX.Equals("somevalue")).Single().Value);
        }

        [Test]
        public void Then_the_dictionary_key_must_be_injected_with_registered_object()
        {
            const string identifier = "myobjectname";
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(identifier)
                .Bind(x => x.PropertyX).To("somevalue");

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectKeyDictionary).ToDefinition(
                    Inline.Dictionary<TestObject, string>()
                        .AddEntry().WithKey<TestObject>(identifier).AndValue("somevalue"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var keyTestObject = applicationContext.GetObject<TestObject>(identifier);
            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectKeyDictionary.ContainsKey(keyTestObject));
            Assert.AreEqual("somevalue", initialisedObject.TestObjectKeyDictionary[keyTestObject]);
        }

        [Test]
        public void Then_the_dictionary_must_be_injected()
        {
            const string identifier = "myobjectname";

            FluentApplicationContext.Register<ObjectWithProperties>(identifier)
                .Bind(x => x.ADictionaryProperty).ToDefinition(
                    Inline.Dictionary<string, string>()
                        .AddEntry().WithKey("myvalue").AndValue("somevalue"));

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = (ObjectWithProperties) applicationContext.GetObject(identifier);

            Assert.IsTrue(initialisedObject.ADictionaryProperty.ContainsKey("myvalue"));
            Assert.AreEqual("somevalue", initialisedObject.ADictionaryProperty["myvalue"]);
        }

        [Test]
        public void Then_the_dictionary_value_and_key_must_be_injected_with_registered_objects()
        {
            const string keyidentifier = "keyidentifier";
            const string valueidentifier = "valueidentifier";
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(keyidentifier)
                .Bind(x => x.PropertyX).To(keyidentifier);
            FluentApplicationContext.Register<TestObject>(valueidentifier)
                .Bind(x => x.PropertyX).To(valueidentifier);

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectKeyValueDictionary).ToDefinition(
                    Inline.Dictionary<TestObject, TestObject>()
                        .AddEntry()
                        .WithKey<TestObject>(keyidentifier)
                        .AndValue<TestObject>(valueidentifier)
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            var keyObject = applicationContext.GetObject<TestObject>(keyidentifier);
            var valueObject = applicationContext.GetObject<TestObject>(valueidentifier);

            Assert.IsTrue(initialisedObject.TestObjectKeyValueDictionary.ContainsKey(keyObject));
            Assert.AreSame(valueObject, initialisedObject.TestObjectKeyValueDictionary[keyObject]);
        }

        [Test]
        public void Then_the_dictionary_value_and_key_must_be_injected_with_registered_types()
        {
            const string keyidentifier = "keyidentifier";
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.PropertyX).To(keyidentifier);

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectKeyValueDictionary).ToDefinition(
                    Inline.Dictionary<TestObject, TestObject>()
                        .AddEntry()
                        .WithKey<TestObject>()
                        .AndValue<TestObject>()
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            var keyObject = applicationContext.GetObject<TestObject>();

            Assert.IsTrue(initialisedObject.TestObjectKeyValueDictionary.ContainsKey(keyObject));
            Assert.AreSame(keyObject, initialisedObject.TestObjectKeyValueDictionary[keyObject]);
        }

        [Test]
        public void Then_the_dictionary_value_must_be_injected_with_inline_object()
        {
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectValueDictionary).ToDefinition(
                    Inline.Dictionary<string, TestObject>()
                        .AddEntry()
                        .WithKey("mykey")
                        .AndValueDefinition(
                            Inline.Object<TestObject>()
                                .Bind(x => x.PropertyX).To("somevalue"))
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectValueDictionary.Values.Where(l => l.PropertyX.Equals("somevalue")).Count() > 0);
            Assert.AreEqual("somevalue", initialisedObject.TestObjectValueDictionary.Where(k => k.Value.PropertyX.Equals("somevalue")).Single().Value.PropertyX);
        }


        [Test]
        public void Then_the_dictionary_value_must_be_injected_with_registered_object()
        {
            const string identifier = "myobjectname";
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>(identifier)
                .Bind(x => x.PropertyX).To("somevalue");

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectValueDictionary).ToDefinition(
                    Inline.Dictionary<string, TestObject>()
                        .AddEntry()
                        .WithKey("mykey")
                        .AndValue<TestObject>(identifier)
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectValueDictionary.Values.Where(l => l.PropertyX.Equals("somevalue")).Count() > 0);
            Assert.AreEqual("somevalue", initialisedObject.TestObjectValueDictionary.Where(k => k.Value.PropertyX.Equals("somevalue")).Single().Value.PropertyX);
        }

        [Test]
        public void Then_the_dictionary_value_must_be_injected_with_registered_type()
        {
            const string dictionaryId = "dicid";

            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.PropertyX).To("somevalue");

            FluentApplicationContext.Register<TestObject>(dictionaryId)
                .Bind(x => x.TestObjectValueDictionary).ToDefinition(
                    Inline.Dictionary<string, TestObject>()
                        .AddEntry()
                        .WithKey("mykey")
                        .AndValue<TestObject>()
                );

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var initialisedObject = applicationContext.GetObject<TestObject>(dictionaryId);

            Assert.IsTrue(initialisedObject.TestObjectValueDictionary.Values.Where(l => l.PropertyX.Equals("somevalue")).Count() > 0);
            Assert.AreEqual("somevalue", initialisedObject.TestObjectValueDictionary.Where(k => k.Value.PropertyX.Equals("somevalue")).Single().Value.PropertyX);
        }
    }
}