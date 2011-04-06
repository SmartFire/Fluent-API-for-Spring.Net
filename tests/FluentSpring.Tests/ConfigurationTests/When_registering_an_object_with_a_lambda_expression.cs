//using System;
//using System.Collections;
//using System.Collections.Generic;
//using FluentSpring.Context;
//using FluentSpring.Tests.TestObjects;
//using NUnit.Framework;
//using FluentSpring.Context.Extensions;
//using Spring.Context;
//using Spring.Objects.Factory;

//namespace FluentSpring.Tests.ConfigurationTests
//{
//    [TestFixture]
//    public class When_registering_an_object_with_a_lambda_expression : FluentContextTestBaseClass
//    {
//        [Test]
//        public void Then_the_object_must_be_registered_when_constructor_used_in_conjunction_with_application_context()
//        {
//            FluentApplicationContext.Register<TestObject>();

//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors(c.GetObject<TestObject>()));

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.IsNotNull(sut.Object);
//            Assert.AreSame(context.GetObject<TestObject>(), sut.Object);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_constructor_used_in_conjunction_with_application_context_and_identifier()
//        {
//            FluentApplicationContext.Register<TestObject>("test");

//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors(c.GetObject<TestObject>("test")));

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.IsNotNull(sut.Object);
//            Assert.AreSame(context.GetObject<TestObject>("test"), sut.Object);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_constructor_used_with_constant_value()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors("some value"));

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreSame("some value", sut.SomeValue);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_object_initialiser_is_used()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                SomeValue = "some value"
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreSame("some value", sut.SomeValue);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_object_initialiser_is_used_with_context()
//        {
//            FluentApplicationContext.Register<TestObject>();
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                Object = c.GetObject<TestObject>()
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.IsNotNull(sut.Object);
//            Assert.AreSame(context.GetObject<TestObject>(), sut.Object);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_object_initialiser_is_used_with_context_and_identifier()
//        {
//            FluentApplicationContext.Register<TestObject>("test");
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                Object = c.GetObject<TestObject>("test")
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.IsNotNull(sut.Object);
//            Assert.AreSame(context.GetObject<TestObject>("test"), sut.Object);
//        }

//        [Test]
//        public void Then_the_object_must_be_registered_when_object_constructor_and_initialiser_is_used()
//        {
//            FluentApplicationContext.Register<TestObject>();
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors(c.GetObject<TestObject>())
//            {
//                SomeValue = "some value"
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreSame("some value", sut.SomeValue);
//            Assert.IsNotNull(sut.Object);
//            Assert.AreSame(context.GetObject<TestObject>(), sut.Object);
//        }

//        [Test]
//        public void Then_the_object_list_must_be_initialised_when_using_the_object_initialiser()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                AList = new List<string>() { "A", "B" }
//            });


//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreEqual("A", sut.AList[0]);
//            Assert.AreEqual("B", sut.AList[1]);
//        }

//        [Test]
//        public void Then_the_custom_object_list_must_be_initialised_when_using_the_object_initialiser()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                AList = new MyList<string>("s") { "A", "B" }
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreEqual("A", sut.AList[0]);
//            Assert.AreEqual("B", sut.AList[1]);
//        }

//        [Test]
//        public void Then_the_array_list_must_be_initialised_when_using_the_object_initialiser()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//            {
//                ArrayList = new string[] { "A", "B" }
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();

//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();

//            Assert.IsNotNull(sut);
//            Assert.AreEqual("A", sut.ArrayList[0]);
//            Assert.AreEqual("B", sut.ArrayList[1]);
//        }

//        [Test]
//        public void Then_circular_references_in_object_initialisers_should_be_initialised_properly()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors
//                                                                                                    {
//                                                                                                        AnotherObjectWithDifferentConstructors = c.GetObject<AnotherObjectWithDifferentConstructors>()
//                                                                                                    });
//            FluentApplicationContext.RegisterByExpression(c => new AnotherObjectWithDifferentConstructors
//            {
//                ObjectWithConstructors = c.GetObject<ObjectWithDifferentConstructors>()
//            });

//            IApplicationContext context = _applicationContextContainer.InitialiseContext();
//            ObjectWithDifferentConstructors sut = context.GetObject<ObjectWithDifferentConstructors>();
//            AnotherObjectWithDifferentConstructors other = context.GetObject<AnotherObjectWithDifferentConstructors>();
//            Assert.IsNotNull(sut);
//            Assert.AreSame(other, sut.AnotherObjectWithDifferentConstructors);
//            Assert.AreSame(sut, other.ObjectWithConstructors);
//        }

//        // you should use property injection for circular references (see test above for example).
//        [Test]
//        public void Then_circular_references_in_object_constructors_cannot_be_initialised_properly()
//        {
//            FluentApplicationContext.RegisterByExpression(c => new ObjectWithDifferentConstructors(c.GetObject<AnotherObjectWithDifferentConstructors>()));
//            FluentApplicationContext.RegisterByExpression(c => new AnotherObjectWithDifferentConstructors(c.GetObject<ObjectWithDifferentConstructors>()));

//            Assert.Throws(typeof(ObjectCurrentlyInCreationException), () => _applicationContextContainer.InitialiseContext());
//        }
//    }

//    public class AnotherObjectWithDifferentConstructors
//    {
//        public AnotherObjectWithDifferentConstructors(ObjectWithDifferentConstructors objectWithDifferentConstructors)
//        {
//            ObjectWithConstructors = objectWithDifferentConstructors;
//        }

//        public AnotherObjectWithDifferentConstructors()
//        {

//        }

//        public ObjectWithDifferentConstructors ObjectWithConstructors
//        {
//            get;
//            set;
//        }
//    }

//    public class MyList<T> : IList<T>
//    {
//        private readonly string _s;

//        public MyList(string s)
//        {
//            _s = s;
//        }

//        public IEnumerator<T> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        public void Add(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Remove(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public int Count
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public bool IsReadOnly
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public int IndexOf(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Insert(int index, T item)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveAt(int index)
//        {
//            throw new NotImplementedException();
//        }

//        public T this[int index]
//        {
//            get { throw new NotImplementedException(); }
//            set { throw new NotImplementedException(); }
//        }
//    }

//    public class ObjectWithDifferentConstructors
//    {
//        public IList<string> AList;

//        public ObjectWithDifferentConstructors(string someValue)
//        {
//            SomeValue = someValue;
//        }

//        public ObjectWithDifferentConstructors(TestObject @object)
//        {
//            Object = @object;
//        }

//        public ObjectWithDifferentConstructors()
//        {
//        }

//        public ObjectWithDifferentConstructors(AnotherObjectWithDifferentConstructors anotherObjectWithDifferentConstructors)
//        {
//            AnotherObjectWithDifferentConstructors = anotherObjectWithDifferentConstructors;
//        }

//        public TestObject Object
//        {
//            get;
//            set;
//        }

//        public string SomeValue
//        {
//            get;
//            set;
//        }

//        public AnotherObjectWithDifferentConstructors AnotherObjectWithDifferentConstructors
//        {
//            get;
//            set;
//        }

//        public string[] ArrayList { get; set; }
//    }
//}
