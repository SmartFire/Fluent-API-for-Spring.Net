namespace FluentSpring.Tests.TestObjects
{
    public class AnotherObjectWithSameInterface : IObjectWithPropertiesInterface
    {
        #region IObjectWithPropertiesInterface Members

        public string AStringProperty { get; set; }

        #endregion
    }
}