using nEkis.Automation.Core.Settings;
using OpenQA.Selenium;
using System;
using System.IO;

namespace $rootnamespace$.Utilities
{
    /// <summary>
    /// Allows to take screenshots
    /// </summary>
    public class Screenshot
    {
        private static string ShotPath { get; set; }
        private static string ShotName
        {
            get
            {
                return string.Format(LogSettings.ScreenShot.Name,
                    TestEnvironment.TestName, DateTime.Now.ToString(DateFormatSettings.ShortDateTime));
            }
        }

        static Screenshot()
        {
            ShotPath = TestEnvironment.TestPath + string.Format(LogSettings.ScreenShot.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));

            if (!Directory.Exists(ShotPath))
                Directory.CreateDirectory(ShotPath);
        }

        /// <summary>
        /// Takes screenshot and saves it in desired location
        /// </summary>
        /// <param name="format">Format of image file</param>
        public static void TakeScreenshot(ScreenshotImageFormat format = ScreenshotImageFormat.Png)
        {
            OpenQA.Selenium.Screenshot shot = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
            shot.SaveAsFile(ShotPath + ShotName, format);
        }
    }
}
