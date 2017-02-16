using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace nEkis.AutomationCore.Test
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
