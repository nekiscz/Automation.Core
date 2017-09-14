using nEkis.Automation.Core.Framework.Objects;
using nEkis.Automation.Core.Settings;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;

namespace $rootnamespace$
{
    /// <summary>
    /// Context of environment and test
    /// </summary>
    public class TestEnvironment
    {
        /// <summary>
        /// Test environments
        /// </summary>
        public enum Environment
        {
            /// <summary>
            /// Stage environment
            /// </summary>
            stage,
            /// <summary>
            /// UAT environment
            /// </summary>
            uat
        }
        /// <summary>
        /// Gets directory to tests (usualy same as dll directory)
        /// </summary>
        public static string TestPath { get; } = TestContext.CurrentContext.TestDirectory;
        /// <summary>
        /// Gets number of failed tests
        /// </summary>
        public static int FailCount { get { return TestContext.CurrentContext.Result.FailCount; } }
        /// <summary>
        /// Gets name of current test
        /// </summary>
        public static string TestName { get { return TestContext.CurrentContext.Test.Name; } }
        /// <summary>
        /// List of failed test names, needs to run IsTestFailed() or SaveFailedTest()
        /// </summary>
        public static List<string> FailedTests { get; set; } = new List<string>();
        /// <summary>
        /// True if test failed
        /// </summary>
        public static bool IsFailed { get { return TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed; } }
        /// <summary>
        /// Contains information about url and user
        /// </summary>
        public static BaseUrl BaseUrl { get; set; }
        /// <summary>
        /// Sets Url variable for future navigation
        /// </summary>
        /// <param name="env">Current test environment</param>
        public static void SelectEnvironment(Environment env)
        {
            BaseUrl = EnvironmentSettings.GetUrl(env.ToString());
        }

        /// <summary>
        /// Gets value representing if the test failed, runs SaveFailedTest() if failed
        /// </summary>
        /// <returns>True if test failed</returns>
        public static bool IsTestFailed()
        {
            var failed = IsFailed;

            if (failed)
                SaveFailedTest();

            return failed;
        }
    }
}
