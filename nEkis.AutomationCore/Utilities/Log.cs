using nEkis.Automation.Core.Settings;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace nEkis.Automation.Core.Utilities
{
    /// <summary>
    /// Allows to log into console and text file
    /// </summary>
    public class Log
    {
        private static bool Verbose { get; set; }

        private static ConsoleTraceListener ctl;
        private static TextWriterTraceListener twtl;
        private static DefaultTraceListener dtl;

        private static string LogPath { get; set; }
        private static string LogFullName { get; set; }
        private static string ReportPath { get; set; }

        private static List<string> FailedTests { get; set; }

        private static DateTime startTime;


        static Log()
        {
            var verbose = TestContext.Parameters.Get("Verbose", "1");
            if (verbose == "0")
                Verbose = false;

            Trace.Listeners.Clear();

            LogPath = CoreProperties.DllPath + string.Format(LogSettings.Log.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));
			ReportPath = CoreProperties.DllPath + string.Format(LogSettings.Report.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

			LogFullName = CoreProperties.DllPath + string.Format(LogSettings.Log.FullName,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));

			if (!Directory.Exists(ReportPath))
                Directory.CreateDirectory(ReportPath);

            twtl = new TextWriterTraceListener(LogFullName);
            ctl = new ConsoleTraceListener(false);
			dtl = new DefaultTraceListener();

			Debug.AutoFlush = true;
            Debug.Listeners.Add(dtl);

			Trace.AutoFlush = true;
            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);
        }

        /// <summary>
        /// Closes txt and console tracers so the files can be used 
        /// </summary>
        public static void CloseTracers()
        {
            twtl.Close();
            ctl.Close();
        }

        /// <summary>
        /// Marks start of testing in console and text files
        /// </summary>
        public static void StartOfFixture()
        {
            startTime = DateTime.Now;
            WriteLine("----------------------------------------------------------------------------------------------------------");
            WriteLine("TESTING STARTED");
            WriteLine($"Local date and time: {startTime.ToString(DateFormatSettings.ReadableDateTime)}");
            WriteLine($"Test directory: {CoreProperties.DllPath}");
            WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Marks end of tesing in console and text files
        /// </summary>
        public static void EndOfFixture()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;

            WriteLine("----------------------------------------------------------------------------------------------------------");
            WriteLine($"Local time: {endTime.ToString(DateFormatSettings.ReadableDateTime)}");
            WriteLine($"Testing took: {duration.ToString("c")} ({duration.TotalSeconds}s)");
            WriteLine($"Number of failed tests: {TestContext.CurrentContext.Result.FailCount.ToString()}");

            if (TestContext.CurrentContext.Result.FailCount > 0)
            {
                WriteLine("Failed tests:");

                foreach (var test in FailedTests)
                {
                    WriteLine($"\t{test}");
                }
            }

            WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Helps trace start of test in logs
        /// </summary>
        public static void StartOfTest()
        {
            WriteLine($"START [{TestContext.CurrentContext.Test.Name}] - {DateTime.Now.ToString(DateFormatSettings.ReadableDateTime)}");
            for (int i = 0; i < 3; i++)
            {

                WriteLine(".");
            }
        }

        /// <summary>
        /// Helps trace end of test in logs
        /// </summary>
        public static void EndOfTest()
        {
            for (int i = 0; i < 3; i++)
            {
                WriteLine(".");
            }
            WriteLine($"END - Test {TestContext.CurrentContext.Test.Name} - {DateTime.Now.ToString(DateFormatSettings.ReadableDateTime)}");
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                WriteLine($"Error message: {TestContext.CurrentContext.Result.Message}\r\n");
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="s">Text to write</param>
        public static void WriteLine(string s)
        {
            Trace.WriteLine(s);
        }

        /// <summary>
        /// Writes line if Verbose is set as true
        /// <para /> By default is Verbose set to true, you can change this from comand line as --params:Verbose=0
        /// </summary>
        /// <param name="s">Text to write</param>
        public static void WriteLineIfVerbose(string s)
        {
            Trace.WriteLineIf(Verbose, s);
        }

		/// <summary>
        /// Writes line into debug tracers
        /// </summary>
        /// <param name="s">Text to write into debug tracers</param>
		public static void PrintLine(string s)
        {
            Debug.WriteLine(s);
        }

        /// <summary>
        /// Saves test name into FailedTests list
        /// </summary>
        public static void SaveFailedTest()
        {
            FailedTests.Add(TestContext.CurrentContext.Test.Name);
        }
    }
}
