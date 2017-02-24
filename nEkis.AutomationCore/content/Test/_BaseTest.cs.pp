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
			Log.StartOfFixture();
            Browser.CreateDriver();
        }

        [SetUp]
        public void BeforeTest()
        {
			Log.StartOfTest();
        }

        [TearDown]
        public void AfterTest()
        {
			Log.EndOfTest();
			if(Environment.IsTestFailed())
				Screenshot.TakeScreenshot();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
		    Log.EndOfFixture();
            Browser.QuitDriver();
        }
    }
}
