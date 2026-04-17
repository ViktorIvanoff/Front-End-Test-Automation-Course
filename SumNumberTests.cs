using OpenQA.Selenium.Chrome;
using NUnit.Framework;

namespace SumNumberPage
{
    public class SumTwoNumbersTest
    {
        private ChromeDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();

        }

        [Test]
        public void Test_Valid_Numbers()
        {
            var sumpage = new SumNumberPage(driver);
            sumpage.OpenPage();
            var result = sumpage.AddNumbers("3", "4");
            Assert.That(result, Is.EqualTo("Sum: 7"));
        }

        [Test]
        public void Test_FirstParamString_SecondParamNumb_InvalidMsg()
        {
            var sumpage = new SumNumberPage(driver);
            sumpage.OpenPage();

            var result = sumpage.AddNumbers("hfhfh", "4");
            Assert.That(result, Is.EqualTo("Sum: invalid input"));
        }

        [Test]
        public void Test_AddTwoNumbers_Reset()
        {
            var sumpage = new SumNumberPage(driver);
            sumpage.OpenPage();

            var result = sumpage.AddNumbers("5", "5");
            bool isReset = sumpage.IsFormEmpty();

            Assert.That(isReset, Is.False);

            sumpage.ResetForm();

            isReset = sumpage.IsFormEmpty();
            Assert.That(isReset, Is.True);
        }

            [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
