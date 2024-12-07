using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumTest
{
    [TestClass]
    public class UnitTest1
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private String webAppUri;

        [TestInitialize]
        public void Setup()
        {
            this.webAppUri = "https://bp-ca1-ks-finalendpoint-epfhcxa8f7brb2fn.z01.azurefd.net/";
        }

        [TestMethod]
        public void TestBP()
        {
            String chromeDriverPath = Environment.GetEnvironmentVariable("ChromeWebDriver") ?? ".";

            using (IWebDriver driver = new ChromeDriver(chromeDriverPath))
            {
                // Navigate to the web app
                driver.Navigate().GoToUrl(webAppUri);

                // Enter systolic and diastolic values
                IWebElement SystolicElement = driver.FindElement(By.Id("BP_Systolic"));
                SystolicElement.Clear(); // Clear existing value
                SystolicElement.SendKeys("110");  // Ideal BP systolic value

                IWebElement DiastolicElement = driver.FindElement(By.Id("BP_Diastolic"));
                DiastolicElement.Clear(); // Clear existing value
                DiastolicElement.SendKeys("75");  // Ideal BP diastolic value

                // Submit the form
                driver.FindElement(By.CssSelector(".btn")).Submit();

                // Explicitly wait for the result
                IWebElement BPValueElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("#form1 > div:nth-child(4)")));

                // Get the text or value attribute
                string bpResult = BPValueElement.GetAttribute("value") ?? BPValueElement.Text;

                // Log the result for debugging
                Console.WriteLine($"BP Result: {bpResult}");

                // Validate the result
                Assert.IsNotNull(bpResult, "BP Result should not be null or empty.");
                StringAssert.EndsWith(bpResult, "Ideal", "Expected BP result to end with 'Ideal'.");

                driver.Quit();
            }
        }
    }
}
