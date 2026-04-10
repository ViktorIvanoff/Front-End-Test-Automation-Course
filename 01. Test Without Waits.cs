using Microsoft.VisualBasic;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Log;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V144.CacheStorage;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SeleniumWait___Lab
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/dynamic.html");
        }

        [Test, Order(1)]
        public void AddBoxWithoutWaitsFails()
        {
            // Click the button to add a new box element.
            // Attempt to find the new box element.
            // Assert that the new box element is displayed
            driver.FindElement(By.Id("adder")).Click();
            IWebElement newBox = driver.FindElement(By.Id("box0"));

            Assert.That(newBox.Displayed);
        }

        [Test, Order(2)]
        public void RevealInputWithoutWaitsFail()
        {
            //Click the button to reveal the input element.
            //Attempt to find the input element.
            //Interact with the input element and assert its value.
            driver.FindElement(By.Id("reveal")).Click();
            IWebElement newInputElement = driver.FindElement(By.Id("revealed"));

            newInputElement.SendKeys("123");
            Assert.That(newInputElement.GetAttribute("value"), Is.EqualTo("123"));
        }

        [Test, Order(3)]
        public void AddBoxWithThreadSleep()
        {
            //  Click the button to add a new box element.
            //  use Thread.Sleep to wait for a fixed amount of time(e.g., 3 seconds).
            // Attempt to find the new box element after the sleep period.
            // Assert that the new box element is displayed.
            driver.FindElement(By.Id("adder")).Click();
            Thread.Sleep(3000);
            IWebElement newBoxElement = driver.FindElement(By.Id("box0"));

            Assert.That(newBoxElement.Displayed);
        }

        [Test, Order(4)]
        public void AddBoxWithImplicitWait()
        {
            // Set an implicit wait of 10 seconds.
            // Click the button to add a new box element.
            // Attempt to find the new box element.
            // Assert that the new box element is displayed.

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.Id("adder")).Click();
            IWebElement newBoxElement = driver.FindElement(By.Id("box0"));

            Assert.That(newBoxElement.Displayed);
        }

        [Test, Order(5)]
        public void RevealInputWithImplicitWaits()
        {
                // Set an implicit wait of 10 seconds.
                //Click the button to reveal the input element.
                //Attempt to find the input element.
                //Assert that the input element exists and is interactable by checking its tag name.
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.Id("reveal")).Click();
            IWebElement newInputElement = driver.FindElement(By.Id("revealed"));

            Assert.That(newInputElement.TagName, Is.EqualTo("input"));

        }

        [Test, Order(6)]
        public void RevealInputWithExplicitWaits()
        {
            // Click the button to reveal the input element.
            // Use an explicit wait to wait for the input element to be displayed.
            // Interact with the input element and assert its value.
            IWebElement revealed = driver.FindElement(By.Id("revealed"));
            driver.FindElement(By.Id("reveal")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            wait.Until(p => revealed.Displayed);

            revealed.SendKeys("123");
            Assert.That(revealed.GetAttribute("value"), Is.EqualTo("123"));
        }

        [Test, Order(7)]
        public void AddBoxWithFluentWaitExpectedConditionsAndIgnoredExceptions()
        {
            //Click the button to reveal the element.
            // Create a WebDriverWait instance with a timeout of 10 seconds and a polling interval of 500 milliseconds.
            // Configure Fluent Wait to ignore specific exceptions.
            // Use ExpectedConditions to wait until the new box element is present and visible.
            // Verify that the newly added box element is displayed.

            driver.FindElement(By.Id("adder")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            IWebElement newBoxElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box0")));
            Assert.That(newBoxElement.Displayed, Is.True);
        }

        [Test, Order(8)]
        public void RevealInputWithCustomFluentWait()
        {
            // Click the button to reveal the input element.
            // Create a WebDriverWait instance with a timeout of 5 seconds and a polling interval of 200 milliseconds.
            // Configure Fluent Wait to ignore specific exceptions.
            // Use a custom wait condition to send keys to the revealed element.
            // The custom wait condition within wait.Until should be a lambda expression that tries to send keys to the revealed element. The wait should continue until the element is interactable and the keys are sent successfully.
            // Verify that the revealed element is displayed and its value is set correctly.

            IWebElement revealed = driver.FindElement(By.Id("revealed"));
            driver.FindElement(By.Id("reveal")).Click();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200),
            };

            wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));

            wait.Until(d =>
            {
                revealed.SendKeys("123");
                return true;
            });

            Assert.That(revealed.TagName, Is.EqualTo("input"));
            Assert.That(revealed.GetAttribute("value"), Is.EqualTo("123"));
        }


        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }

    }
}
