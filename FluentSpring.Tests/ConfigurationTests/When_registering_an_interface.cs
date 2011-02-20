using FluentSpring.Context;
using FluentSpring.Context.Extensions;
using FluentSpring.Tests.TestObjects;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_registering_an_interface : FluentContextTestBaseClass
    {
        public static bool ConditionalFlag;

        [Test]
        public void Then_I_should_be_able_to_get_the_object_instance()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().AsNonSingleton();
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>().To<ObjectWithProperties>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.IsNotNull(objectInstance);
        }

        [Test]
        public void Then_I_should_be_able_to_register_several_objects_to_the_same_interface()
        {
            FluentApplicationContext.Register<ObjectWithProperties>();
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>();

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>().To<ObjectWithProperties>().When(() => true);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>().To<AnotherObjectWithSameInterface>().When(() => false);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.IsNotNull(objectInstance);
        }

        [Test]
        public void Then_it_should_only_return_the_registered_object_which_is_available_by_condition()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().Bind(o => o.AStringProperty).To("validobject");
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>().Bind(o => o.AStringProperty).To("invalidobject");

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>().To<ObjectWithProperties>().When(() => true);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>().To<AnotherObjectWithSameInterface>().When(() => false);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("validobject", objectInstance.AStringProperty);
        }

        [Test]
        public void Then_it_should_only_return_the_registered_object_which_is_available_by_condition_and_the_first_one_it_finds()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().Bind(o => o.AStringProperty).To("invalidobject");
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>().Bind(o => o.AStringProperty).To("validobject");

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>()
                    .To<ObjectWithProperties>().When(() => false);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>()
                .To<AnotherObjectWithSameInterface>().When(() => true);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("validobject", objectInstance.AStringProperty);
        }

        [Test]
        public void Then_when_the_condition_result_changes_but_the_mapping_is_static_then_it_should_return_the_original_object()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().Bind(o => o.AStringProperty).To("object");
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>().Bind(o => o.AStringProperty).To("anotherobject");
            ConditionalFlag = true;

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>(true).To<ObjectWithProperties>().When(() => !ConditionalFlag);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>(true).To<AnotherObjectWithSameInterface>().When(() => ConditionalFlag);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("anotherobject", objectInstance.AStringProperty);

            ConditionalFlag = false;

            objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("anotherobject", objectInstance.AStringProperty);
        }

        [Test]
        public void Then_when_the_condition_result_changes_it_should_return_the_other_object()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().Bind(o => o.AStringProperty).To("object");
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>().Bind(o => o.AStringProperty).To("anotherobject");
            ConditionalFlag = true;

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>(false).To<ObjectWithProperties>().When(() => !ConditionalFlag);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>(false).To<AnotherObjectWithSameInterface>().When(() => ConditionalFlag);

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("anotherobject", objectInstance.AStringProperty);

            ConditionalFlag = false;

            objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreEqual("object", objectInstance.AStringProperty);
        }

        [Test]
        public void Then_the_object_properties_must_bound_to_the_registered_interface()
        {
            FluentApplicationContext.Register<ObjectWithProperties>().Bind(o => o.AStringProperty).To("object");
            FluentApplicationContext.Register<AnotherObjectWithSameInterface>().Bind(o => o.AStringProperty).To("anotherobject");
            
            ConditionalFlag = true;

            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>()
                .To<ObjectWithProperties>().When(() => !ConditionalFlag);
            FluentApplicationContext.Bind<IObjectWithPropertiesInterface>()
                .To<AnotherObjectWithSameInterface>().When(() => ConditionalFlag);

            FluentApplicationContext.Register<TestObject>()
                .Bind(x => x.SomeObject).To<IObjectWithPropertiesInterface>();

            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();

            var testObject = applicationContext.GetObject<TestObject>();
            var objectInstance = applicationContext.GetObject<IObjectWithPropertiesInterface>();

            Assert.AreSame(objectInstance, testObject.SomeObject);
        }
    }
}