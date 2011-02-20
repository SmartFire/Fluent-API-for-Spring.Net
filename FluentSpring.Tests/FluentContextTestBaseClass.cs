using FluentSpring.Context;
using FluentSpring.Tests.Spring;
using NUnit.Framework;

namespace FluentSpring.Tests
{
    [TestFixture]
    public abstract class FluentContextTestBaseClass
    {
        #region Setup/Teardown

        [SetUp]
        public void Given_a_testable_spring_application_context()
        {
            AbstractSpringContextTests.ClearContextCache();

            _applicationContextContainer = new SpringApplicationContextTester(new[] {"assembly://FluentSpring.Tests/FluentSpring.Tests.SpringXmlConfiguration/springobjects.xml"});
            FluentApplicationContext.Clear();
        }

        #endregion

        protected SpringApplicationContextTester _applicationContextContainer;
    }
}