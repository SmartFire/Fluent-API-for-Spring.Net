using FluentSpring.Tests.Spring;
using NUnit.Framework;
using Spring.Context;

namespace FluentSpring.Tests.ConfigurationTests
{
    [TestFixture]
    public class When_initialising_a_test_application_context
    {
        #region Setup/Teardown

        [SetUp]
        public void Given_a_testable_spring_application_context()
        {
            AbstractSpringContextTests.ClearContextCache();
            _applicationContextContainer = new SpringApplicationContextTester(new[] {"assembly://FluentSpring.Tests/FluentSpring.Tests.SpringXmlConfiguration/springobjects.xml"});
        }

        #endregion

        private SpringApplicationContextTester _applicationContextContainer;

        [Test]
        public void It_must_load_the_application_context()
        {
            IApplicationContext applicationContext = _applicationContextContainer.InitialiseContext();
            Assert.IsNotNull(applicationContext);
        }
    }
}