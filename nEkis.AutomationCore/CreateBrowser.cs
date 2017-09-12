using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using nEkis.Automation.Core.Utilities;

namespace nEkis.Automation.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBrowser 
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SelectedBrowser
        {
            /// <summary>
            /// GeckoDriver
            /// </summary>
            Firefox,
            /// <summary>
            /// ChromeDriver
            /// </summary>
            Chrome,
            /// <summary>
            /// InternetExplorerServer
            /// </summary>
            IE,
            /// <summary>
            /// PhantomJSDriver
            /// </summary>
            PhantomJS
        }

        /// <summary>
        /// Creates instance of driver based on user input
        /// </summary>
        /// <param name="browser">String representation of driver</param>
        /// <returns>Instance of IWebDriver</returns>
        public static IWebDriver Create(string browser)
        {
            Log.WriteLineIfVerbose($"Trying to find browser coresponding to parameter '{browser}'");

            var selectedBrowser = SelectBrowser(browser);

            Log.WriteLineIfVerbose($"Selected browser {selectedBrowser.ToString()}, starting the browser");

            switch (selectedBrowser)
            {
                case SelectedBrowser.Firefox:
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                    service.FirefoxBinaryPath = $"{TestContext.CurrentContext.TestDirectory}\\geckodriver.exe";
                    return new FirefoxDriver(service);
                case SelectedBrowser.Chrome:
                    return new ChromeDriver();
                case SelectedBrowser.IE:
                    Log.WriteLineIfVerbose("Make shure that IE driver is set to 100% zoom and Protected Mode is off in Security tab!");
                    return new InternetExplorerDriver();
                case SelectedBrowser.PhantomJS:
                    return new PhantomJSDriver();
                default:
                    return new ChromeDriver();
            }
        }

        #region PrivateMethods

        private static SelectedBrowser SelectBrowser(string browser)
        {
            switch (browser.ToLower())
            {
                case "ch":
                case "chrome":
                case "googlechrome":
                    return SelectedBrowser.Chrome;
                case "ie":
                case "explorer":
                case "internetexplorer":
                    Log.WriteLineIfVerbose("Make shure that IE driver is set to 100% zoom and Protected Mode is off in Security tab!");
                    return SelectedBrowser.IE;
                case "ff":
                case "firefox":
                case "mozilla":
                case "mozillafirefox":
                    return SelectedBrowser.Firefox;
                case "ph":
                case "phantom":
                case "phantomjs":
                case "headless":
                    return SelectedBrowser.PhantomJS;
                default:
                    return SelectedBrowser.Chrome;
            }
        }

        #endregion
    }
}
