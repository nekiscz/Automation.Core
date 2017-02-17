using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nEkis.Automation.Core.Utilities
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
                return string.Format(@ConfigurationManager.AppSettings["screenshotname"],
                    Environment.TestName, DateTime.Now.ToString(Environment.DateTimeFormat));
            }
        }

        static Screenshot()
        {
            ShotPath = Environment.TestPath + string.Format(@ConfigurationManager.AppSettings["screenshotdirectory"],
                DateTime.Now.ToString(Environment.DateFormat));

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
