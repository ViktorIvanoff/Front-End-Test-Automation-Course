using OpenQA.Selenium;

namespace IdeaCenterPom.Pages
{
    public class IdeasReadPage : BasePage
    {
        public IdeasReadPage(IWebDriver driver) : base(driver)
        {
        }

        public string Url = BaseUrl + "/Ideas/Create";

        public IWebElement IdeaTitle =>
           driver.FindElement(By.XPath("//h1[@class='mb-0 h4']"));

        public IWebElement IdeaDescription =>
           driver.FindElement(By.XPath("//p[@class='offset-lg-3 col-lg-6']"));
    }
}
