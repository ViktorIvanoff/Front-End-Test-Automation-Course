using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace IdeaCenterPom.Pages
{
    public class MyIdeasPage : BasePage
    {
        public MyIdeasPage(IWebDriver driver) : base(driver)
        {
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        WebDriverWait Wait;

        public string Url = BaseUrl + "/Ideas/MyIdeas";

        public ReadOnlyCollection<IWebElement> IdeaCards => this.Wait.Until(driver => driver.FindElements(By.XPath("//div[@class='card mb-4 box-shadow']")));

        public IWebElement ViewButtonLastCard => IdeaCards.Last().FindElement(By.XPath(".//a[contains(@href, '/Ideas/Read')]"));

        public IWebElement IdeaDescriptionLastCard => IdeaCards.Last().FindElement(By.XPath(".//p[@class='card-text']"));

        public IWebElement EditButtonLastCard => IdeaCards.Last().FindElement(By.XPath(".//a[contains(@href, '/Ideas/Edit')]"));

        public IWebElement DeleteButtonLastCard => IdeaCards.Last().FindElement(By.XPath(".//a[contains(@href, '/Ideas/Delete')]"));

        public void OpenPage()
        { 
            driver.Navigate().GoToUrl(Url);
        }
    }
}
