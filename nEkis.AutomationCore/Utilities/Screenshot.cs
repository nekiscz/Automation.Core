using nEkis.Automation.Core.Settings;
using OpenQA.Selenium;
using System;
using System.IO;

namespace nEkis.Automation.Core.Utilities
{
    /// <summary>
    /// Allows to take screenshots
    /// </summary>
    public class ScreenShot
    {
        private IWebDriver browser;
        private string ShotPath => CoreProperties.DllPath + string.Format(LogSettings.ScreenShot.Path,
                DateTime.Now.ToString(DateFormatSettings.ShortDate));

        /// <summary>
        /// Takes screenshots of browser viewport
        /// </summary>
        /// <param name="browser">Browser to take screenshots from</param>
        public ScreenShot(IWebDriver browser)
        {
            this.browser = browser;
            if (!Directory.Exists(this.ShotPath))
                Directory.CreateDirectory(this.ShotPath);
        }

        /// <summary>
        /// Takes screenshot and saves it in desired location
        /// </summary>
        /// <param name="testName">Name of test inserted into screenshot name</param>
        /// <param name="format">Format of image file</param>
        public void TakeScreenshot(string testName, ScreenshotImageFormat format = ScreenshotImageFormat.Png)
        {
            var shotName = string.Format(LogSettings.ScreenShot.Name, testName, DateTime.Now.ToString(DateFormatSettings.ShortDateTime));

            OpenQA.Selenium.Screenshot shot = ((ITakesScreenshot)browser).GetScreenshot();
            shot.SaveAsFile(this.ShotPath + shotName, format);
        }
    }
}
