using OpenQA.Selenium;

namespace IdeaCenterPom.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public string Url = BaseUrl + "/Users/Login";

        public IWebElement EmailField =>
           driver.FindElement(By.XPath("//input[@name='Email']"));

        public IWebElement PasswordField =>
           driver.FindElement(By.XPath("//input[@name='Password']"));

        public IWebElement SignInButton =>
           driver.FindElement(By.XPath("//button[@class='btn btn-primary btn-lg btn-block']"));


        public void Login(string email, string password)
        {
            EmailField.SendKeys(email);
            PasswordField.SendKeys(password);
            SignInButton.Click();
        }

        public void OpenPage()
        { 
            driver.Navigate().GoToUrl(Url);
        }
    }
}