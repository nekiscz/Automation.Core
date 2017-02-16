using nEkis.Automation.Core;
using NUnit.Framework;

namespace $rootnamespace$.Tests
{
    class BaseTest
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            Browser.CreateDriver(AvailableBrowsers.Chrome);
        }

        [SetUp]
        public void BeforeTest()
        {

        }

        [TearDown]
        public void AfterTest()
        {

        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            Browser.QuitDriver();
        }

    }
}
