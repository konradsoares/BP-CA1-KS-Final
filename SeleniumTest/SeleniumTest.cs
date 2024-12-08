using Microsoft.VisualStudio.TestTools.UnitTesting;

// NuGet install Selenium WebDriver package and Support Classes
 
using OpenQA.Selenium;

// NuGet install Chrome Driver
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

// run 2 instances of VS to do run Selenium tests against localhost
// instance 1 : run web app e.g. on IIS Express
// instance 2 : from Test Explorer run Selenium test
// or use the dotnet vstest task
// e.g. dotnet vstest SeleniumTest\bin\debug\netcoreapp2.1\seleniumtest.dll /Settings:SeleniumTest.runsettings

namespace SeleniumTest
{
    [TestClass]
    public class UnitTest1
    {
        // .runsettings file contains test run parameters
        // e.g. URI for app
        // test context for this run

        private TestContext testContextInstance;

        // test harness uses this property to initliase test context
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        // URI for web app being tested
        private String webAppUri;

        // .runsettings property overriden in vsts test runner 
        // release task to point to run settings file
        // also webAppUri overriden to use pipeline variable

        [TestInitialize]                // run before each unit test
        public void Setup()
        {
            // read URL from SeleniumTest.runsettings (configure run settings)
            //this.webAppUri = testContextInstance.Properties["webAppUri"].ToString();

            this.webAppUri = "https://bp-ca1-ks-final-dev-efbdh3a8hfgufufu.germanywestcentral-01.azurewebsites.net/";
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
                driver.FindElement(By.CssSelector(".btn.btn-default")).Click();

                // Explicitly wait for the result
                IWebElement BPValueElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(c => c.FindElement(By.CssSelector("#form1 > div:nth-child(4) > input")));

                // Get the value attribute instead of text
                string bpResult = BPValueElement.Text.ToString();

                // Validate the result
                StringAssert.EndsWith(bpResult, "Ideal");

                driver.Quit();

                // alternative - use Cypress or Playright
            }
        }
    }
}
