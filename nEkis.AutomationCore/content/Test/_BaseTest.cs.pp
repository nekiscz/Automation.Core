using $rootnamespace$.Utilities;
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
			TestEnvironment.SelectEnvironment(TestEnvironment.Environment.stage);
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
			if(TestEnvironment.IsTestFailed())
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
