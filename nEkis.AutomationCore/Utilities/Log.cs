using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace nEkis.Automation.Core.Utilities
{
    /// <summary>
    /// Centralized way to log into console and text file
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Listner for console
        /// </summary>
        private static ConsoleTraceListener ctl { get; set; }
        /// <summary>
        /// Listner for txt file
        /// </summary>
        private static TextWriterTraceListener twtl { get; set; }

        /// <summary>
        /// Fullpath to log file
        /// </summary>
        private static string LogPath { get; set; }

        private static DateTime startTime;

        static Log()
        {

            Trace.Listeners.Clear();

            LogPath = Environment.TestPath + @ConfigurationManager.AppSettings["logdirectory"];

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            LogPath = LogPath + "\\" + ConfigurationManager.AppSettings["logname"];

            twtl = new TextWriterTraceListener(LogPath);
            ctl = new ConsoleTraceListener(false);

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
            WriteLine("Local date and time: " + startTime.ToString("d. M. yyyy H:m:s"));
            WriteLine("Test directory: " + Environment.TestPath);
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
            WriteLine("Local time: " + endTime.ToString("d. M. yyyy H:m:s"));
            WriteLine("Testing took: " + duration.ToString("c") + " (" + duration.TotalSeconds + "s)");
            WriteLine("Number of failed tests: " + Environment.FailCount.ToString());

            if (Environment.FailedTests.Count > 0)
            {
                WriteLine("Failed tests:");

                foreach (var test in Environment.FailedTests)
                {
                    WriteLine("\t{0}", test);
                }
            }

            WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Helps trace start of test in logs
        /// </summary>
        public static void StartOfTest()
        {
            WriteLine("START [{0}] - {1}", new object[] { Environment.TestName, DateTime.Now.ToString("dd. MM. yyyy HH:mm:ss") });
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
            WriteLine("END - Test {0} - {1}", new object[] { Environment.TestName, DateTime.Now.ToString("dd. MM. yyyy HH:mm:ss") });
            if (Environment.IsTestFailed())
                WriteLine("Error message: {0}\r\n", TestContext.CurrentContext.Result.Message);
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        public static void WriteLine(string s)
        {
            Trace.WriteLine(s);
            Trace.Flush();
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Argument to be formated with text</param>
        public static void WriteLine(string text, object arg)
        {
            WriteLine(string.Format(text, arg));
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Array of arguments to be formated with text</param>
        public static void WriteLine(string text, params object[] arg)
        {
            WriteLine(string.Format(text, arg));
        }

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Browser.Random.Next(s.Length)]).ToArray());
        }
    }
}
