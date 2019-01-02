using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Remote;


namespace POM
{
    [TestFixture]
    public class TestFirefox
    {
        IWebDriver firefoxDriver;
        TutByLoginPage objLogin;
        TutByLogOutPage objLogout;

        [SetUp]
        public void Setup()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable("irinamakarevich", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable("2acad7c0-0b62-49ed-ba78-47fb33494aa2", EnvironmentVariableTarget.User);

            var caps = new FirefoxOptions();

            caps.AddAdditionalCapability(CapabilityType.Version, "39.0", true);
            caps.AddAdditionalCapability(CapabilityType.Platform, "Windows 8.1", true);
            caps.AddAdditionalCapability("username", "irinamakarevich", true);
            caps.AddAdditionalCapability("accessKey", "2acad7c0-0b62-49ed-ba78-47fb33494aa2", true);
            //caps.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);
            firefoxDriver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability.ToCapabilities());
            objLogin = new TutByLoginPage(firefoxDriver);
            objLogout = new TutByLogOutPage(firefoxDriver);
            firefoxDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            firefoxDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            firefoxDriver.Navigate().GoToUrl("https://tut.by");

        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)firefoxDriver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            if (firefoxDriver != null)
                firefoxDriver.Quit();
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
