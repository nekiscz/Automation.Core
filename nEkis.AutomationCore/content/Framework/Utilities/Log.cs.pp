using nEkis.Automation.Core.Settings;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace $rootnamespace$.Utilities
{
    /// <summary>
    /// Allows to log into console and text file
    /// </summary>
    public class Log
    {
        private static bool Verbose { get; set; }
        /// <summary>
        /// Listner for console
        /// </summary>
        private static ConsoleTraceListener ctl { get; set; }
        /// <summary>
        /// Listner for txt file
        /// </summary>
        private static TextWriterTraceListener twtl { get; set; }
        /// <summary>
        /// Default listener for VS console
        /// </summary>
		private static DefaultTraceListener dtl { get; set; }

        /// <summary>
        /// Fullpath to log file
        /// </summary>
        private static string LogPath { get; set; }
        private static string LogFullName { get; set; }
        private static string ReportPath { get; set; }

        private static DateTime startTime;

        static Log()
        {
            var verbose = TestContext.Parameters.Get("Verbose", "1");
            if (verbose == "0")
                Verbose = false;

            Trace.Listeners.Clear();

            LogPath = TestEnvironment.TestPath + string.Format(LogSettings.Log.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));
			ReportPath = TestEnvironment.TestPath + string.Format(LogSettings.Report.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

			LogFullName = TestEnvironment.TestPath + string.Format(LogSettings.Log.FullName,
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
            WriteLine($"Test directory: {TestEnvironment.TestPath}");
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
            WriteLine($"Number of failed tests: {TestEnvironment.FailCount.ToString()}");

            if (TestEnvironment.FailedTests.Count > 0)
            {
                WriteLine("Failed tests:");

                foreach (var test in TestEnvironment.FailedTests)
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
            WriteLine($"START [{TestEnvironment.TestName}] - {DateTime.Now.ToString(DateFormatSettings.ReadableDateTime)}");
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
            WriteLine($"END - Test {TestEnvironment.TestName} - {DateTime.Now.ToString(DateFormatSettings.ReadableDateTime)}");
            if (TestEnvironment.IsFailed)
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
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Argument to be formated with text</param>
        [System.Obsolete("Use method WriteLine with $\"{method}\" connotation")]
        public static void WriteLine(string text, object arg)
        {
            WriteLine(string.Format(text, arg));
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Array of arguments to be formated with text</param>
        [System.Obsolete("Use method WriteLine with $\"{method}\" connotation")]
        public static void WriteLine(string text, params object[] arg)
        {
            WriteLine(string.Format(text, arg));
        }
    }
}
