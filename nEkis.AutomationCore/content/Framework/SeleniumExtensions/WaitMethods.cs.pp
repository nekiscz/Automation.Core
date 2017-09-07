using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace $rootnamespace$
{
    public static class WaitMethods
    {
        /// <summary>
        /// Waits till the element is clickable
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillClickable(this IWebElement element)
        {
            Browser.Wait.Until(ExpectedConditions.ElementToBeClickable(element));
            return element;
        }

        /// <summary>
        /// Waits till element disapeares from DOM
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillNotPresent(this IWebElement element)
        {
            Browser.Wait.Until((d) => !element.IsPresent());
            return element;
        }

        /// <summary>
        /// Waits till element is displayed
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillVisible(this IWebElement element)
        {
            Browser.Wait.Until((d) => element.IsDisplayed());
            return element;
        }

        /// <summary>
        /// Waits till element is not displayed
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillNotVisible(this IWebElement element)
        {
            Browser.Wait.Until((d) => !element.IsDisplayed());
            return element;
        }

        /// <summary>
        /// Waits there are more then one option in selectbox
        /// </summary>
        /// <param name="element">Any HTML element wraping these options, usually select element</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillOptionsPresent(this IWebElement element)
        {
            Browser.Wait.Until((d) => element.FindElements(By.TagName("option")).Count > 1);
            return element;
        }

        /// <summary>
        /// Waits till at least one element in list 
        /// </summary>
        /// <param name="elements">Any HTML element</param>
        /// <returns>Given element</returns>
        public static IList<IWebElement> WaitTillListItemsPresent(this IList<IWebElement> elements)
        {
            Browser.Wait.Until((d) => elements.Count > 0);
            return elements;
        }

        /// <summary>
        /// Waits till element stops moving
        /// </summary>
        /// <param name="element">Any HTML element</param>
        /// <param name="timeout">Frequency of checks, by defauld 150ms</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillStopsMoving(this IWebElement element, int timeout = 150)
        {
            var loc = element.Location;
            Browser.Wait.Until((d) =>
            {
                System.Threading.Thread.Sleep(timeout);
                var newloc = element.Location;

                if (loc == newloc)
                    return true;
                else
                {
                    loc = newloc;
                    return false;
                }
            });

            return element;
        }

        /// <summary>
        /// Waits till text in element changes
        /// </summary>
        /// <param name="element">Inputs, selects and elemrnt with value</param>
        /// <param name="timeout">Frequency of checks, by defauld 150ms</param>
        /// <returns>Given element</returns>
        public static IWebElement WaitTillTextChanges(this IWebElement element, int timeout = 150)
        {
            var text = element.GetText();
            Browser.Wait.Until((d) =>
            {
                System.Threading.Thread.Sleep(timeout);
                var newtext = element.GetText();

                if (text == newtext)
                    return true;
                else
                {
                    text = newtext;
                    return false;
                }
            });

            return element;
        }
    }
}
