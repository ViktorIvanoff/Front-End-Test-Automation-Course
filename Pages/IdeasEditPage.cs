using OpenQA.Selenium;

namespace IdeaCenterPom.Pages
{
    public class IdeasEditPage : BasePage
    {
        public IdeasEditPage(IWebDriver driver) : base(driver)
        {
        }

        public string Url = BaseUrl + "/Ideas/Create";

        public IWebElement IdeaTitle =>
           driver.FindElement(By.XPath("//input[@id='form3Example1c']"));

        public IWebElement IdeaDescription =>
           driver.FindElement(By.XPath("//textarea[@id='form3Example4cd']"));

        public IWebElement EditButton =>
           driver.FindElement(By.XPath("//button[text()='Edit']"));
    }
}
