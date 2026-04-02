global using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V143.Network;


namespace _03._Practice_Locators
{
    public class Tests
    {

        private IWebDriver driver;
        private string baseUrl = "C:\\Users\\solev\\OneDrive\\Desktop\\Front-End Test Automation - October 2025\\03. Selenium WebDriver\\My Solutions\\03. Practice Locators\\SimpleForm\\Locators.html";

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(baseUrl);
        }

        [Test]
        public void Test1()
        {

            driver.FindElement(By.Id("lname"));
            driver.FindElement(By.Name("newsletter"));
            driver.FindElement(By.TagName("a"));
            driver.FindElement(By.ClassName("information"));
        }

        [Test]
        public void Test2()
        { 
            driver.FindElement(By.LinkText("SoftUni Official Page"));
            driver.FindElement(By.PartialLinkText("Official Page"));

        }

        public void Test3()
        {
            driver.FindElement(By.CssSelector("#fname"));
            driver.FindElement(By.CssSelector("input[name='fname']"));
            driver.FindElement(By.CssSelector("input[class*='button']"));
            driver.FindElement(By.CssSelector("div.additional-info > p > input[type='text']"));
            driver.FindElement(By.CssSelector("form div.additional-info input[type='text']"));
        }

        [Test]
        public void Test4()
        {
            driver.FindElement(By.XPath("/html/body/form/input[1]"));
            driver.FindElement(By.XPath("//input[@value='m']"));
            driver.FindElement(By.XPath("//input[@name='lname']"));
            driver.FindElement(By.XPath("//input[@type='checkbox']"));
            driver.FindElement(By.XPath("//input[@class='button']"));
            driver.FindElement(By.XPath("//div[@class='additional-info']//input[@type='text']"));
        }

        [OneTimeTearDown]

        public void TearDown()
        {
            driver.Dispose();
        }

    }
}