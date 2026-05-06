using IdeaCenterPom.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace IdeaCenterPom.Tests
{
    public class BaseTest
    {
        public IWebDriver driver;
        public static string lastCreatedIdeaTitle;
        public static string lastCreatedIdeaDescription;
        public LoginPage loginPage;
        public CreateIdeaPage createIdeaPage;
        public MyIdeasPage myIdeasPage;
        public IdeasReadPage ideasReadPage;
        public IdeasEditPage ideasEditPage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            loginPage = new LoginPage(driver);
            createIdeaPage = new CreateIdeaPage(driver);
            myIdeasPage = new MyIdeasPage(driver);
            ideasReadPage = new IdeasReadPage(driver);
            ideasEditPage = new IdeasEditPage(driver);
            // Log in to the application
            loginPage.OpenPage();
            loginPage.Login("testEmail@mail.com", "testtest1");
        }

        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                TakeScreenshot(TestContext.CurrentContext.Test.Name);
            }
            driver.Quit();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void TakeScreenshot(string testName)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string screenshotName = $"{testName}_{timestamp}.png";
                string filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, screenshotName);
                screenshot.SaveAsFile(filePath);
                Console.WriteLine($"Screenshot saved as {filePath}");
            }
            catch (WebDriverException e)
            {
                Console.WriteLine($"Failed to take screenshot: {e}");
            }
        }
    }
}
