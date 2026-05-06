using OpenQA.Selenium;

namespace IdeaCenterPom.Pages
{
    public class CreateIdeaPage : BasePage
    {
        public CreateIdeaPage(IWebDriver driver) : base(driver)
        {
        }

        public string Url = BaseUrl + "/Ideas/Create";

        public IWebElement TitleInput =>
           driver.FindElement(By.XPath("//input[@id='form3Example1c']"));

        public IWebElement ImageInput =>
           driver.FindElement(By.XPath("//input[@id='form3Example3c']"));

        public IWebElement DescriptionInput =>
           driver.FindElement(By.XPath("//textarea[@id='form3Example4cd']"));

        public IWebElement CreateButton =>
           driver.FindElement(By.XPath("//button[@class='btn btn-primary btn-lg']"));

        public IWebElement MainErrorMessage =>
           driver.FindElement(By.XPath("//div[@class='text-danger validation-summary-errors']//li"));

        public IWebElement TitleErrorMessage => driver.FindElements(By.XPath("//span[@class='text-danger field-validation-error']"))[0];

        public IWebElement DescriptionErrorMessage => driver.FindElements(By.XPath("//span[@class='text-danger field-validation-error']"))[1];

        public void CreateIdea(string title, string imageurl, string description)
        {
            TitleInput.SendKeys(title);
            ImageInput.SendKeys(imageurl);
            DescriptionInput.SendKeys(description);
            CreateButton.Click();
        }

        public void AssertErrorMessages()
        {
            Assert.True(MainErrorMessage.Text.Equals("Unable to create new Idea!"), "Main error message is not as expected");

            Assert.True(TitleErrorMessage.Text.Equals("The Title field is required."), "Title error message is not as expected");

            Assert.True(DescriptionErrorMessage.Text.Equals("The Description field is required."), "Description error message is not as expected");
        }

        public void OpenPage()
        {
            driver.Navigate().GoToUrl(Url);
        }
    }
}
