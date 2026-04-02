global using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Security.Cryptography.X509Certificates;

namespace _02._First_Test
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void Test1()
        {
            // take out url separately in a variable
            driver.Url = "https://nakov.com.";
            var windowTitle = driver.Title;
            Assert.That(windowTitle.Contains("Svetlin Nakov – Official Web Site"));
            Console.WriteLine(windowTitle);

            var searchLink = driver.FindElement(By.ClassName("smoothScroll"));
            //Find the search link by its class name and verify its text contains "SEARCH".
            Assert.That(searchLink.Text.Contains("SEARCH"));
            Console.WriteLine(searchLink.Text);

            searchLink.Click();

            var message = driver.FindElement(By.Id("s"));
            //Retrieve and verify the placeholder attribute of the search input is "Search this site".
            var placeholderText = message.GetAttribute("placeholder");
            Assert.That(placeholderText, Is.EqualTo("Search this site"));
            Console.WriteLine(placeholderText);
        }

            [TearDown]
            public void TearDown()
            {
                driver.Dispose();

            }
    }
}
