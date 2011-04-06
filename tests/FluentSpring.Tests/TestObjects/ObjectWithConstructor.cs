using System;

namespace FluentSpring.Tests.TestObjects
{
    public class ObjectWithConstructor
    {
        private readonly IObjectWithPropertiesInterface _objectInterface;
        private readonly string _first;
        private readonly int _second;
        private readonly string _third;

        public ObjectWithConstructor(string first, int second, string third)
        {
            _first = first;
            _second = second;
            _third = third;
        }

        public ObjectWithConstructor(string first, int second)
        {
            _first = first;
            _second = second;
        }

        public ObjectWithConstructor(string first)
        {
            _first = first;
        }

        public ObjectWithConstructor(TestObject testObject)
        {
            TestObject = testObject;
        }

        public ObjectWithConstructor(IObjectWithPropertiesInterface objectInterface)
        {
            _objectInterface = objectInterface;
        }

        public ObjectWithConstructor(TestObject testObject, string first)
        {
            TestObject = testObject;
            _first = first;
        }

        public ObjectWithConstructor()
        {
            
        }

        public string First
        {
            get { return _first; }
        }

        public IObjectWithPropertiesInterface ObjectInferface
        {
            get { return _objectInterface; }
        }

        public int Second
        {
            get { return _second; }
        }

        public string Third
        {
            get { return _third; }
        }

        public TestObject TestObject { get; set; }
    }
}