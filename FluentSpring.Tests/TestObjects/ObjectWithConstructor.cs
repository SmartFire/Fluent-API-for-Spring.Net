namespace FluentSpring.Tests.TestObjects
{
    public class ObjectWithConstructor
    {
        private readonly string _first;
        private readonly int _second;
        private readonly TestObject _testObject;
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
            _testObject = testObject;
        }

        public ObjectWithConstructor(TestObject testObject, string first)
        {
            _testObject = testObject;
            _first = first;
        }

        public string First
        {
            get { return _first; }
        }

        public int Second
        {
            get { return _second; }
        }

        public string Third
        {
            get { return _third; }
        }

        public TestObject TestObject
        {
            get { return _testObject; }
        }
    }
}