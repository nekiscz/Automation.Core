using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nEkis.AutomationCore
{
    class Environment
    {
        public static string TestPath { get; set; } = TestContext.CurrentContext.TestDirectory;
        public static int FailCount { get { return TestContext.CurrentContext.Result.FailCount; } }
        public static string  TestName { get { return TestContext.CurrentContext.Test.Name; } }
        public static List<string> FailedTests { get; set; }

        /// <summary>
        /// Gets value representing if the test failed
        /// </summary>
        /// <returns>True if test failed</returns>
        public static bool IsTestFailed()
        {
            var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;

            if (failed)
                FailedTests.Add(TestContext.CurrentContext.Test.Name);

            return failed;
        }


    }
}
