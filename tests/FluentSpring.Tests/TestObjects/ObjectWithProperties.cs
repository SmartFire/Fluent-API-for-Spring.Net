using System;
using System.Collections.Generic;

namespace FluentSpring.Tests.TestObjects
{
    public class ObjectWithProperties : IObjectWithPropertiesInterface
    {
        public int AIntegerProperty { get; set; }
        public DateTime ADateTimeProperty { get; set; }
        public IDictionary<string, string> ADictionaryProperty { get; set; }
        public TestObject TestObject { get; set; }

        #region IObjectWithPropertiesInterface Members

        public string AStringProperty { get; set; }

        #endregion
    }
}