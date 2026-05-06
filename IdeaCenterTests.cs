using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace IdeaCenterNoPom
{
    [TestFixture]
    public class IdeaCenterTests
    {
        protected IWebDriver driver;
        private static readonly string BaseUrl = "http://144.91.123.158:82";
        private static string? lastCreatedIdeaTitle;
        #pragma warning disable CS8618 
        private static string lastCreatedIdeaDescription;
        #pragma warning restore CS8618 

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Log in to the application
            driver.Navigate().GoToUrl($"{BaseUrl}/Users/Login");
            driver.FindElement(By.Id("typeEmailX-2")).SendKeys("idea@ideacenter.com");
            driver.FindElement(By.Id("typePasswordX-2")).SendKeys("123456");
            driver.FindElement(By.CssSelector("button.btn-primary")).Click();
        }

        [Test, Order(1)]
        public void CreateIdeaWithInvalidDataTest()
        {
            string invalidTitle = "";
            string invalidDescription = "";

            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/Create");

            driver.FindElement(By.Id("form3Example1c")).SendKeys(invalidTitle);
            driver.FindElement(By.Id("form3Example4cd")).SendKeys(invalidDescription);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            string currentUrl = driver.Url;
            Assert.That(currentUrl, Is.EqualTo($"{BaseUrl}/Ideas/Create"), "The page should remain on the creation page with invalid data.");

            var mainErrorMessage = driver.FindElement(By.CssSelector(".validation-summary-errors li"));
            var requiredFieldsErrors = driver.FindElements(By.XPath("//span[@class='text-danger field-validation-error']"));
            Assert.Multiple(() =>
            {
                Assert.That(mainErrorMessage.Text.Trim(), Is.EqualTo("Unable to create new Idea!"), "The main error message is not displayed as expected.");

                Assert.That(requiredFieldsErrors[0].Text.Equals("The Title field is required."), Is.True, "Title field required error not displayed");
                Assert.True(requiredFieldsErrors[1].Text.Equals("The Description field is required."), "Description field required error not displayed");
            });

        }

        [Test, Order(2)]
        public void CreateRandomIdeaTest()
        {
            lastCreatedIdeaTitle = "Idea " + GenerateRandomString(5);
            lastCreatedIdeaDescription = "Description " + GenerateRandomString(10);

            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/Create");

            driver.FindElement(By.Id("form3Example1c")).SendKeys(lastCreatedIdeaTitle);
            driver.FindElement(By.Id("form3Example3c")).SendKeys("http://www.pictures.com/picture.jpg");
            driver.FindElement(By.Id("form3Example4cd")).SendKeys(lastCreatedIdeaDescription);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            string expectedUrl = $"{BaseUrl}/Ideas/MyIdeas";
            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "The URL after creation did not match the expected URL.");

            var ideaCards = driver.FindElements(By.CssSelector(".card.mb-4.box-shadow"));
            var lastIdeaCard = ideaCards.Last();
            var ideaDescriptionElement = lastIdeaCard.FindElement(By.CssSelector("p.card-text"));

            Assert.That(ideaDescriptionElement.Text.Trim(), Is.EqualTo(lastCreatedIdeaDescription), "The new idea was not found or is incorrectly listed in 'My Ideas'.");
        }

        [Test, Order(3)]
        public void ViewLastCreatedIdeaTest()
        {
            Assert.That(lastCreatedIdeaTitle, Is.Not.Null, "No title set for the last created idea.");

            Console.WriteLine("Viewing idea with title: " + lastCreatedIdeaTitle);

            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/MyIdeas");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var ideaCards = wait.Until(driver => driver.FindElements(By.XPath("//div[@class='card mb-4 box-shadow']")));

            Assert.That(ideaCards.Count, Is.GreaterThan(0), "No idea cards were found on the page.");

            var lastIdeaCard = ideaCards.Last();
            var viewButton = lastIdeaCard.FindElement(By.XPath(".//a[contains(@href, '/Ideas/Read')]"));

            viewButton.Click();

            var ideaTitleElement = driver.FindElement(By.CssSelector("h1.mb-0.h4"));

            // Assertion to verify the title of the idea
            string ideaTitle = ideaTitleElement.Text.Trim();
            Assert.That(ideaTitle, Is.EqualTo(lastCreatedIdeaTitle), "The title of the idea does not match the expected value.");

        }

        [Test, Order(4)]
        public void EditLastCreatedIdeaTitleTest()
        {
            Assert.That(lastCreatedIdeaTitle, Is.Not.Null, "No title set for the last created idea.");

            Console.WriteLine("Editing idea with title: " + lastCreatedIdeaTitle);

            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/MyIdeas");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var ideaCards = wait.Until(driver => driver.FindElements(By.CssSelector(".card.mb-4.box-shadow")));

            Assert.IsTrue(ideaCards.Count > 0, "No idea cards were found on the page.");

            var lastIdeaCard = ideaCards.Last();
            var editButton = lastIdeaCard.FindElement(By.CssSelector("a[href*='/Ideas/Edit']"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(editButton).Click().Perform();
            Console.WriteLine("Clicked 'Edit' button using Actions class.");

            var titleInput = driver.FindElement(By.Id("form3Example1c"));
            string newTitle = "Changed Title: " + lastCreatedIdeaTitle;
            titleInput.Clear();
            titleInput.SendKeys(newTitle);

            var editSubmitButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            editSubmitButton.Click();
            Console.WriteLine("Clicked 'Edit' button to save changes.");

            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/MyIdeas");

            ideaCards = wait.Until(driver => driver.FindElements(By.CssSelector(".card.mb-4.box-shadow")));
            lastIdeaCard = ideaCards.Last();
            var viewButton = lastIdeaCard.FindElement(By.CssSelector("a[href*='/Ideas/Read']"));
            actions.MoveToElement(viewButton).Click().Perform();
            Console.WriteLine("Clicked 'View' button to verify the edited idea.");

            var ideaTitleElement = driver.FindElement(By.CssSelector("h1.mb-0.h4"));
            string ideaTitle = ideaTitleElement.Text.Trim();
            Assert.That(ideaTitle, Is.EqualTo(newTitle), "The title of the idea does not match the expected value.");
        }

        [Test, Order(5)]
        public void EditIdeaDescriptionTest()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/MyIdeas");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var ideaCards = wait.Until(driver => driver.FindElements(By.CssSelector(".card.mb-4.box-shadow")));

            Assert.That(ideaCards.Count > 0, Is.True, "No idea cards were found on the page.");

            var lastIdeaCard = ideaCards.Last();
            var editButton = lastIdeaCard.FindElement(By.CssSelector("a[href*='/Ideas/Edit']"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(editButton).Click().Perform();

            string newDescription = "Changed Description: " + lastCreatedIdeaDescription;

            var descriptionField = wait.Until(driver => driver.FindElement(By.Id("form3Example4cd")));
            descriptionField.Clear();
            descriptionField.SendKeys(newDescription);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            string expectedUrl = $"{BaseUrl}/Ideas/MyIdeas";
            Assert.That(driver.Url, Is.EqualTo(expectedUrl), "The URL after editing did not match the expected URL.");

            ideaCards = wait.Until(driver => driver.FindElements(By.CssSelector(".card.mb-4.box-shadow")));
            lastIdeaCard = ideaCards.Last();
            var ideaDescriptionElement = lastIdeaCard.FindElement(By.CssSelector("p.card-text"));

            Assert.That(ideaDescriptionElement.Text.Trim(), Is.EqualTo(newDescription), "The description of the idea did not update as expected.");
        }

        [Test, Order(6)]
        public void DeleteLastIdeaTest()
        {
            driver.Navigate().GoToUrl($"{BaseUrl}/Ideas/MyIdeas");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var ideaCards = wait.Until(driver => driver.FindElements(By.CssSelector(".card.mb-4.box-shadow")));

            Assert.That(ideaCards.Count > 0, Is.True, "No idea cards were found on the page.");

            var lastIdeaCard = ideaCards.Last();
            var deleteButton = lastIdeaCard.FindElement(By.CssSelector("a[href*='/Ideas/Delete']"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(deleteButton).Click().Perform();


            ideaCards = driver.FindElements(By.CssSelector(".card.mb-4.box-shadow"));

            bool isIdeaDeleted = ideaCards.All(static card => !card.Text.Contains(lastCreatedIdeaDescription));
            Assert.That(isIdeaDeleted, Is.True, "The idea was not deleted successfully or is still visible in the list.");
        }
      
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
