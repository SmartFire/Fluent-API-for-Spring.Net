using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace FluentSpring.Tests.TestObjects
{
    public class TestObject
    {
        public static bool DestroyedFlag;

        public String PropertyX { get; set; }
        public IDictionary<string, String> SomeDictionary { get; set; }
        public IDictionary<TestObject, string> TestObjectKeyDictionary { get; set; }
        public IDictionary<string, TestObject> TestObjectValueDictionary { get; set; }
        public IDictionary<TestObject, TestObject> TestObjectKeyValueDictionary { get; set; }

        public IList<string> ListOfString { get; set; }
        public IList<TestObject> ListOfTestObjects { get; set; }

        public NameValueCollection NameValueCollection { get; set; }

        public void Initialise()
        {
            PropertyX = "InitialisedWithMethod";
        }

        public void Destroyed()
        {
            DestroyedFlag = true;
        }

        public IObjectWithPropertiesInterface SomeObject { get; set; }
    }
}