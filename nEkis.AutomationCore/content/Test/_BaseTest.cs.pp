using nEkis.Automation.Core;
using nEkis.Automation.Core.Utilities;
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
			if(Environment.IsTestFailed())
				Screenshot.TakeScreenshot();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            Browser.QuitDriver();
        }

    }
}
