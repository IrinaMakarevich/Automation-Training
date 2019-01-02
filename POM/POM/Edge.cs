using System;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Remote;


namespace POM
{
    [TestFixture]
    public class TestEdge
    {
        IWebDriver edgeDriver;
        TutByLoginPage objLogin;
        TutByLogOutPage objLogout;

        [SetUp]
        public void Setup()
        {
            //TODO please supply your Sauce Labs user name in an environment variable
            var sauceUserName = Environment.GetEnvironmentVariable("irinamakarevich", EnvironmentVariableTarget.User);
            //TODO please supply your own Sauce Labs access Key in an environment variable
            var sauceAccessKey = Environment.GetEnvironmentVariable("2acad7c0-0b62-49ed-ba78-47fb33494aa2", EnvironmentVariableTarget.User);

            var caps = new EdgeOptions();

            caps.AddAdditionalCapability(CapabilityType.Version, "latest");
            caps.AddAdditionalCapability(CapabilityType.Platform, "Windows 10");
            caps.AddAdditionalCapability("username", "irinamakarevich");
            caps.AddAdditionalCapability("accessKey", "2acad7c0-0b62-49ed-ba78-47fb33494aa2");
            //caps.AddAdditionalCapability("name", TestContext.CurrentContext.Test.Name, true);
            edgeDriver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), caps.ToCapabilities(),
                TimeSpan.FromSeconds(600));
            //driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capability.ToCapabilities());
            objLogin = new TutByLoginPage(edgeDriver);
            objLogout = new TutByLogOutPage(edgeDriver);
            edgeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            edgeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            edgeDriver.Navigate().GoToUrl("https://tut.by");

        }

        [TearDown]
        public void CleanUpAfterEveryTestMethod()
        {
            var passed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Passed;
            ((IJavaScriptExecutor)edgeDriver).ExecuteScript("sauce:job-result=" + (passed ? "passed" : "failed"));
            if (edgeDriver != null)
                edgeDriver.Quit();
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

