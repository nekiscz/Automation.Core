using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using nEkis.Automation.Core.Framework.Objects;
using nEkis.Automation.Core.Utilities;

namespace nEkis.Automation.Core
{
    /// <summary>
    /// Everything connected with driver
    /// </summary>
    public class Browser : IWebDriver
    {
        #region Custom prop

        private IWebDriver Driver { get; set; }

        private EventFiringWebDriver Edr { get; set; }

        public BaseUrl BaseUrl { get; private set; }

        /// <summary>
        /// Allows actions in driver
        /// </summary>
        public Actions ActionsBuilder { get; set; }
        /// <summary>
        /// Wait for driver actions
        /// </summary>
        public WebDriverWait Wait { get; set; }
        /// <summary>
        /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
        /// </summary>
        public Random Random { get; set; }
        /// <summary>
        /// JavaScript Executor 
        /// </summary>
        public IJavaScriptExecutor JsExecutor { get; set; }

        #endregion

        #region IWebDriver props

        public string Url { get { return this.Driver.Url; } set { this.Driver.Url = value; } }

        public string Title { get { return this.Driver.Title; } }

        public string PageSource { get { return this.Driver.PageSource; } }

        public string CurrentWindowHandle { get { return this.Driver.CurrentWindowHandle; } }

        public ReadOnlyCollection<string> WindowHandles { get { return this.Driver.WindowHandles; } }

        #endregion

        /// <summary>
        /// Creates Driver (Chrome by default) and Event Firing Driver, creates rules for exceptions and events, wait set to 20s by default
        /// </summary>
        /// <param name="waitsec">Timeout setting for WebDriverWait</param>
        /// <param name="defaultBrowser">Sets default browser that will be run if none is given</param>
        public Browser(string defaultBrowser = "ch", int waitsec = 20)
        {
            Init(defaultBrowser, waitsec);
        }

        public Browser(BaseUrl baseUrl, string defaultBrowser = "ch", int waitsec = 20)
        {
            this.BaseUrl = baseUrl;
            Init(defaultBrowser, waitsec);
        }

        private void Init(string defaultBrowser, int waitsec)
        {
            this.Driver = CreateBrowser.Create(defaultBrowser);

            Log.WriteLine($"Driver created ({this.Driver.GetType().Name})");
            this.Edr = new EventFiringWebDriver(this.Driver);
            Log.WriteLine("Event firing driver created");

            this.Edr.ExceptionThrown += this.Edr_ExceptionThrown;
            this.Edr.Navigating += this.Edr_Navigating;
            this.Edr.ElementClicking += this.Edr_ElementClicking;
            this.Edr.ElementValueChanged += this.Edr_ElementValueChanged;

            this.Driver = this.Edr;
            Log.WriteLine("Event firing driver added to driver");

            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(waitsec));
            this.ActionsBuilder = new Actions(this.Driver);
            this.Random = new RandomGenerator();
            this.JsExecutor = (IJavaScriptExecutor)this.Driver;
        }

        /// <summary>
        /// Deletes all cookies
        /// </summary>
        public void ClearCookies()
        {
            this.Driver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Gets string URL from driver
        /// </summary>
        /// <returns>Current URL</returns>
        public string GetUrl()
        {
            return this.Driver.Url;
        }

        /// <summary>
        /// Gets source code of current page
        /// </summary>
        /// <returns>Whole source, innetHTML and outerHTML</returns>
        public string GetSource()
        {
            return this.Driver.PageSource;
        }

        /// <summary>
        /// Accepts brower JS alert
        /// </summary>
        public void AcceptAlert()
        {
            this.Driver.SwitchTo().Alert().Accept();
        }

        /// <summary>
        /// Send keys to JS alert
        /// </summary>
        /// <param name="keys">Text to be inserted</param>
        public void SendKeysAlert(string keys)
        {
            this.Driver.SwitchTo().Alert().SendKeys(keys);
        }

        /// <summary>
        /// Navigates to URL
        /// </summary>
        /// <param name="url">URL to navigate to</param>
        public void GoToUrl(string url)
        {
            const string urlPrefixes = "(https?|file)://";

            if (Regex.IsMatch(url, urlPrefixes))
                this.Driver.Url = url;
            else this.Driver.Url = $"{BaseUrl.Url}{url}";
        }

        /// <summary>
        /// Navigates back one entry in history
        /// </summary>
        public void GoBack()
        {
            this.Driver.Navigate().Back();
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        public void Refresh()
        {
            this.Driver.Navigate().Refresh();
        }

        /// <summary>
        /// Switches browser back to default content (out of any iframe)
        /// </summary>
        public void SwitchToDefault()
        {
            this.Driver.SwitchTo().DefaultContent();
        }

        /// <summary>
        /// Gets cookie by name
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        /// <returns>Returns cookie as object by its name</returns>
        public Cookie GetCookie(string name)
        {
            return this.Driver.Manage().Cookies.GetCookieNamed(name);
        }

        /// <summary>
        /// Deletes all cookies from browser
        /// </summary>
        public void ClearAllCookies()
        {
            this.Driver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Maximizes browser window
        /// </summary>
        public void Maximize()
        {
            this.Driver.Manage().Window.Maximize();
        }

        #region EvenFiringDriver
        /// <summary>
        /// Is fired when value of element is changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the element</param>
        private void Edr_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            try
            {
                string text = e.Element.GetText();
                if (string.IsNullOrEmpty(text))
                    Log.WriteLineIfVerbose($"// Cleared elements value or no text put in: {e.Element.GetAttribute("outerHTML")}");
                else
                    Log.WriteLineIfVerbose($"// Changed value: '{text}' of element '{e.Element.GetAttribute("outerHTML")}'");

            }
            catch (Exception ex) when (ex is StaleElementReferenceException || ex is NoSuchElementException)
            {
                Log.WriteLineIfVerbose($"// Element is no longer present in DOM and can't be logged ({ex.Message})");
            }
        }

        /// <summary>
        /// Is fired when you click on something
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the element</param>
        private void Edr_ElementClicking(object sender, WebElementEventArgs e)
        {
            string elementText = string.Empty;

            if (!string.IsNullOrEmpty(e.Element.GetText()))
                elementText = e.Element.GetText();
            else if (!string.IsNullOrEmpty(e.Element.GetAttribute("value")))
                elementText = e.Element.GetAttribute("value");

            Log.WriteLine($"// Clicked on element: '{elementText}' ({e.Element.GetAttribute("outerHTML")})");
        }

        /// <summary>
        /// Is fired when you navigate to some URL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the browser</param>
        private void Edr_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            Log.WriteLine($"// Navigating to URL: {e.Url}");
        }

        /// <summary>
        /// Is fired when exeption in test is thrown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Object representing the browser</param>
        private void Edr_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            Log.WriteLine("! Exception in test, message: " + e.ThrownException.Message);
        }

        #endregion

        #region IWebDriver implementations


        /// <summary>
        /// Closes driver - profile is not deleted from temp, window is just closed
        /// </summary>
        public void Close()
        {
            if (this.Driver != null)
            {
                this.Driver.Manage().Cookies.DeleteAllCookies();
                foreach (var handle in this.Driver.WindowHandles)
                {
                    this.Driver.SwitchTo().Window(handle);
                    this.Driver.Close();
                }

                this.Driver = null;
            }
        }

        /// <summary>
        /// Quits driver (closes window and deletes profile from temp), closes logs
        /// </summary>
        public void Quit()
        {
            try
            {
                AcceptAlert();
            }
            catch (Exception)
            {
                Log.WriteLine("No alert blocking driver");
            }

            if (this.Driver != null)
            {
                this.Driver.Quit();
                this.Driver = null;
                Log.WriteLine("Driver closed and profile deleted");
            }

            Log.CloseTracers();
        }


        public IOptions Manage()
        {
            return this.Driver.Manage();
        }

        public INavigation Navigate()
        {
            return this.Driver.Navigate();
        }

        public ITargetLocator SwitchTo()
        {
            return this.Driver.SwitchTo();
        }

        public IWebElement FindElement(By by)
        {
            return this.Driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.Driver.FindElements(by);
        }

        public void Dispose()
        {
            this.Driver.Dispose();
        }
        #endregion
    }
}
