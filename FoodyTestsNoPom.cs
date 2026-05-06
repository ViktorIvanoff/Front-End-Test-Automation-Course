using Microsoft.VisualBasic;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V145.CacheStorage;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net;
using System.Net.Http.Headers;
using OpenQA.Selenium.Interactions;
using System.Reflection.Emit;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;



namespace FoodyTests
{
    public class Tests
    {
        private ChromeDriver driver;

        private readonly string BaseUrl = "http://144.91.123.158:81/";

        private string? lastCreatedFoodName;

        private string? lastCreatedFoodDescription;

        private Random random;

        [OneTimeSetUp]
        public void Setup()
        {
            random = new Random();
    
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_leak_detection", false);

            driver = new ChromeDriver(chromeOptions);

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //Login to the system
            driver.Navigate().GoToUrl(BaseUrl + "User/Login");
            driver.FindElement(By.XPath("//input[@name='Username']")).SendKeys("JARKO");
            driver.FindElement(By.XPath("//input[@name='Password']")).SendKeys("6786788");

            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
        }

        [Test, Order(1)]
        public void AddFoodWithIvalidData_Test()
        {
            //Arrange
            string invalidFoodName = "";
            string invalidFoodDescrioption = "";

            driver.FindElement(By.XPath("//a[@href='/Food/Add']")).Click();

            //Act
            driver.FindElement(By.CssSelector("[name='Name']")).SendKeys(invalidFoodName);
            driver.FindElement(By.CssSelector("[name='Description']")).SendKeys(invalidFoodDescrioption);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            Assert.That(driver.Url, Is.EqualTo(BaseUrl + "Food/Add"));

            var errorMessage = driver.FindElement(By.XPath("//div[@class='text-danger validation-summary-errors']//li")).Text;

            Assert.That(errorMessage.Trim(), Is.EqualTo("Unable to add this food revue!"));
        }

        [Test, Order(2)]
        public void AddFoodWithValidData_Test()
        {
            //Arrange
                //Fill out the form with randomly generated data: 
                //Use a helper method to generate unique titles and descriptions.
            lastCreatedFoodName = "Title_" + random.Next(999, 99999).ToString();
            lastCreatedFoodDescription = "Description_"+ random.Next(999, 99999).ToString();

            driver.FindElement(By.XPath("//a[text()='Add Food']")).Click();

            //Act

            driver.FindElement(By.CssSelector("[name='Name']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.CssSelector("[name='Description']")).SendKeys(lastCreatedFoodDescription);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Asserts
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            var homePageElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//h1[text()='FOODY']")));

            Assert.That(driver.Url, Is.EqualTo(BaseUrl));
            Assert.That(driver.Title, Is.EqualTo("Home Page - Foody.WebApp"));

            var lastCreatedFood = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//h2"));

            Assert.That(lastCreatedFood.Text, Is.EqualTo(lastCreatedFoodName));
        }

        [Test, Order(3)]
        public void EditLastCreatedFood()
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();
            var editedName = "Edited";

            var lastFoodEditButton = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//a[text()='Edit']"));

            //// Scroll element into view to avoid MoveTargetOutOfBoundsException
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", lastFoodEditButton);
            Thread.Sleep(200); // Small delay to ensure scroll completes

            lastFoodEditButton.Click();

            //Act
            driver.FindElement(By.CssSelector("[name='Name']")).SendKeys(editedName);
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //Assert
            var lastCreatedFoodNameText = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//h2")).Text;

            Assert.That(lastCreatedFoodNameText, Is.EqualTo(lastCreatedFoodName));

            Console.WriteLine("The title remains unchaged due to unimplemented functionallity");
        }

        [Test, Order(4)]
        public void SerachForFoodTest()
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();

            //Act
            driver.FindElement(By.CssSelector("[name='keyword']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.CssSelector("[type='submit']")).Click();

            //Assert
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.UrlContains($"keyword={lastCreatedFoodName}"));

            var allFoodContainers = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']"));

            Assert.That(allFoodContainers.Count(), Is.EqualTo(1));

            var lastCreatedFoodNameText = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//h2")).Text;

            Assert.That(lastCreatedFoodNameText, Is.EqualTo(lastCreatedFoodName));
        }

        [Test, Order(5)]
        public void DeleteLastCreatedFood()
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();

            var initialCount = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']")).Count();

            var lastFoodContainer = driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]"));

            // Scroll element into view to avoid MoveTargetOutOfBoundsException
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", lastFoodContainer);
            Thread.Sleep(200); // Small delay to ensure scroll completes

            Assert.That(lastFoodContainer.Enabled, Is.True);
            Assert.That(lastFoodContainer.Displayed, Is.True);

            //Act
            driver.FindElement(By.XPath("(//div[@class='row gx-5 align-items-center'])[last()]//a[text()='Delete']")).Click();

            //Asserts
            var countAfterDeletion = driver.FindElements(By.XPath("//div[@class='row gx-5 align-items-center']")).Count();

            Assert.That(countAfterDeletion, Is.EqualTo(initialCount - 1));
        }

        [Test, Order(6)]
        public void SearchForDeletedFood()
        {
            //Arrange
            driver.FindElement(By.XPath("//a[text()='FOODY']")).Click();

            //Act
            driver.FindElement(By.CssSelector("[name='keyword']")).SendKeys(lastCreatedFoodName);
            driver.FindElement(By.CssSelector("[type='submit']")).Click();

            //Asserts
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            // Wait for either the empty plate image or the "no foods" message to appear
            wait.Until(driver => 
            {
                try
                {
                    var emptyPlate = driver.FindElement(By.CssSelector("[alt='empty plate picture']"));
                    return emptyPlate.Displayed;
                }
                catch
                {
                    try
                    {
                        var noResultsMessage = driver.FindElement(By.XPath("//h2[@class='display-4']"));
                        return noResultsMessage.Displayed && noResultsMessage.Text.Contains("no foods");
                    }
                    catch
                    {
                        return false;
                    }
                }
            });

            var noResultsMessage = driver.FindElement(By.XPath("//h2[@class='display-4']"));
            var addFoodButton = driver.FindElement(By.XPath("//a[text()='Add food']"));

            Assert.That(noResultsMessage.Text, Is.EqualTo("There are no foods :("));
            Assert.That(addFoodButton.Displayed, Is.True);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        { 
            driver.Quit();
            driver.Dispose();
        }
    }
}