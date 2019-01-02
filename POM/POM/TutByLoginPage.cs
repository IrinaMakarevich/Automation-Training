using OpenQA.Selenium;
using NUnit.Framework;

namespace POM
{
    public class TutByLoginPage
    {
        IWebDriver driver;
        private By loginButton = By.ClassName("enter");
        private By usernameField = By.Name("login");
        private By passwordField = By.XPath("//input[@type='password']");
        private By submitButton = By.CssSelector("input[type='submit']");
        private By usernameButton = By.CssSelector("span[class='uname']");

        public TutByLoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        //Open Login form
        public void OpenLoginForm()
        {
            driver.FindElement(loginButton).Click();
        }
        //Fill Username field
        public void SetUserName(string username)
        {
            driver.FindElement(usernameField).SendKeys(username);
        }
        //Fill Password filed 
        public void SetPassword(string password)
        {
            driver.FindElement(passwordField).SendKeys(password);
        }
        //click on Submit button
        public void ClickSubmit()
        {
            driver.FindElement(submitButton).Click();

        }
        //Get the name of user
        public string GetUsername()
        {
            return driver.FindElement(usernameButton).Text;
        }
        //login method
        public void LoginToTutBy(string username, string password)
        {
            this.OpenLoginForm();
            this.SetUserName(username);
            this.SetPassword(password);
            this.ClickSubmit();

        }
    }
}
