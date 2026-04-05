using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net;
using System.Xml.Linq;

namespace _02._Working_with_Web_Tables
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            // in the setup method we make new instance of driver
            driver = new ChromeDriver();

            // we set the timeout
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // we navigate to the url of the app
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
        }

        [Test]
        public void FillingUpTableData()
        {
            // locate web table on page using Xpath
            IWebElement productTable = driver.FindElement(By.XPath("//*[@id='bodyContent']/div/div[2]/table"));
            ReadOnlyCollection<IWebElement> tableRows = productTable.FindElements(By.XPath("//tbody/tr"));

            //Create a CSV file to save product information
            string path = System.IO.Directory.GetCurrentDirectory() + "/productinformation.csv";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Traverse through table rows to find the table columns
            //Loop through each row and then each column within the row

            foreach (IWebElement trow in tableRows)
            {
                ReadOnlyCollection<IWebElement> tableCols = trow.FindElements(By.XPath("td"));

                foreach (IWebElement tcol in tableCols)
                {
                    //Extract the text from each cell, split the text to separate product name and cost, and format it.
                    String data = tcol.Text;
                    String[] productInfo = data.Split("\n");
                    String product = productInfo[0].Trim() + "," + productInfo[1].Trim() + "\n";

                    File.AppendAllText(path, product);
                }
            }

            Assert.That(File.Exists(path), Is.True, "CSV file was not created.");
            Assert.That(new FileInfo(path).Length, Is.GreaterThan(0), "CSV file is empty.");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }
    }
}
