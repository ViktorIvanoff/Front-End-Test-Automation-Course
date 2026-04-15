using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net;
using System.Xml.Linq;
using SeleniumExtras.WaitHelpers;

namespace SeleniumWaitsExercise
{
    public class Tests
    {

        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
        }

        [Test, Order(1)]
        public void SearchProduct_Keyboard_ShouldAddToCart()
        {
            driver.FindElement(By.Name("keywords")).SendKeys("keyboard");
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            //try catch block
            try
            {
                driver.FindElement(By.Id("tdb4")).Click();

                //assertion
                Assert.That(driver.PageSource.Contains("keyboard"), "The product is not found");
                //Assert.That(driver.PageSource.Does.Contain("keyboard"), "The product is not found");

            }
            catch (Exception ex)
            {
                {
                    Assert.Fail("Unexpected exception" + ex.Message);
                }
            }
        }

        [Test, Order(2)]
        public void SearchNonExistentProduct_Junk_ShouldThrowNoSuchElementException()
        {
            //locate and fill up input field for product
            driver.FindElement(By.Name("keywords")).SendKeys("junk");
            // click search btn
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            //try catch catch block
            try
            {
                driver.FindElement(By.Id("tdb4")).Click();
                Assert.That(driver.PageSource.Contains("keyboard"), "The product is not found");
                //Assert.That(driver.PageSource.Does.Contain("keyboard"), "The product is not found");
            }
            catch (NoSuchElementException ex)
            {
                Assert.Pass("Expected NoSuchElementException was thrown");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {

                Assert.Fail("Unexpected error" + ex.Message);
            }
        }
            [TearDown]
            public void TearDown()
            {
                driver.Dispose();
            }

        }
}

