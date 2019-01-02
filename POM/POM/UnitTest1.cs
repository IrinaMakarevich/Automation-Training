using System;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.IO;
using System.Drawing;
using Allure.NUnit.Attributes;
using Allure.Commons;
using OpenQA.Selenium.Remote; 


namespace POM
{
    [TestFixture]
    public class TestLoginAndLogout 
    {
        IWebDriver driver;
        TutByLoginPage objLogin;
        TutByLogOutPage objLogout;

        [SetUp]
        public void Setup()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable("irinamakarevich", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable("2acad7c0-0b62-49ed-ba78-47fb33494aa2", EnvironmentVariableTarget.User);

            var caps = new ChromeOptions();

            caps.AddAdditionalCapability(CapabilityType.Version, "40.0", true);
            caps.AddAdditionalCapability(CapabilityType.Platform, "Linux", true);
            caps.AddAdditionalCapability("username", "irinamakarevich", true);
            caps.AddAdditionalCapability("accessKey", "2acad7c0-0b62-49ed-ba78-47fb33494aa2", true);
            //caps.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);
            driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability.ToCapabilities());
            objLogin = new TutByLoginPage(driver);
            objLogout = new TutByLogOutPage(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            driver.Navigate().GoToUrl("https://tut.by");
            
        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)driver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            if (driver != null)
                driver.Quit();
        }


        [TestCase("seleniumtests@tut.by", "123456789zxcvbn")]
        public void TestUserIsLoggedIn(string un, string pwd)
        {
            objLogin.LoginToTutBy(un, pwd);
            Assert.AreEqual(objLogin.GetUsername(), "Selenium Test", "Result text is wrong");
        }

        
        [TestCase("seleniumtests@tut.by", "123456789zxcvbn")]
        public void TestUserIsLoggedOut(string un, string pwd)
        {
            objLogin.LoginToTutBy(un, pwd);
            objLogout.LogOutTutBy();
            Assert.Throws<NoSuchElementException>(() => objLogout.GetUsername());
        }
        
    }
}
