using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace _01._Handling_Form_Input
{
    public class Tests
    {

        // 1. above SetUp method we initialize WebDriver
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            // in the setup method we make new instance of driver
            driver = new ChromeDriver();

            // we set the timeout
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // we navigate to the url of the app
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
        }

        [Test]
        public void RegisterUser()
        {

            // Click on the My Account link
            driver.FindElement(By.Id("tdb3")).Click();
            driver.FindElement(By.Id("tdb4")).Click();

            // fill the form
            driver.FindElement(By.CssSelector("input[type='radio'][value='m']")).Click();
            driver.FindElement(By.Name("firstname")).SendKeys("Ivan");
            driver.FindElement(By.Name("lastname")).SendKeys("Gogov"); 
            driver.FindElement(By.Name("dob")).SendKeys("05/21/1970");

            //generate unique email 
            Random rnd = new Random();
            int num = rnd.Next(1000, 9999);
            String email = "sect" + num.ToString() + "@homail.com";

            driver.FindElement(By.Name("email_address")).SendKeys(email);
            driver.FindElement(By.Name("company")).SendKeys("SoftUni");
            driver.FindElement(By.Name("street_address")).SendKeys("ulica");
            driver.FindElement(By.Name("suburb")).SendKeys("A1");
            driver.FindElement(By.Name("postcode")).SendKeys("1234");
            driver.FindElement(By.Name("city")).SendKeys("Sofia");
            driver.FindElement(By.Name("state")).SendKeys("A2");

            new SelectElement(driver.FindElement(By.Name("country"))).SelectByText("Angola");

            // IWebElement countryDropdown = driver.FindElement(By.Name("country"))
            // SelectElement selectCountry = new SelectElement(countrydropdown);
            // selectCountry.SelectByText("Angola");

            driver.FindElement(By.Name("telephone")).SendKeys("5656");
            driver.FindElement(By.Name("newsletter")).Click();

            driver.FindElement(By.Name("password")).SendKeys("123456");
            driver.FindElement(By.Name("confirmation")).SendKeys("123456");

            driver.FindElement(By.Id("tdb4")).Submit();

            Assert.That(driver.PageSource.Contains("Your Account Has Been Created!"), "Account creation failed.");

            driver.FindElement(By.LinkText("Log Off")).Click();
            driver.FindElement(By.Id("tdb4")).Click();
        }

        // dont forget the TearDown method
        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }
    }
}
