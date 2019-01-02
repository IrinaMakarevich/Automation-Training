using OpenQA.Selenium;
using NUnit.Framework;

namespace POM
{
    public class TutByLogOutPage
    {
        IWebDriver driver;
        private By usernameButton = By.CssSelector("span[class='uname']");
        private By logOut = By.CssSelector("div.b-popup-i>a");

        public TutByLogOutPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        //open Log Out form
        public void OpenLogoutForm()
        {
            driver.FindElement(usernameButton).Click();
        }
        //click on logout button
        public void ClickLogoutButton()
        {
            driver.FindElement(logOut).Click();
        }
        //Get the name of user
        public string GetUsername()
        {
            return driver.FindElement(usernameButton).Text;
        }
        //method for logout
        public void LogOutTutBy()
        {
            this.OpenLogoutForm();
            this.ClickLogoutButton();
        }
          
    }
}
