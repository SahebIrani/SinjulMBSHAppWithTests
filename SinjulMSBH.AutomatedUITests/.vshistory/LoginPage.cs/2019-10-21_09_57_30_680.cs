using System.IO;
using System.Reflection;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Xunit;

namespace SinjulMSBH.AutomatedUITests
{
    public class LoginPage
    {
        IWebDriver Driver;

        [Fact]
        public void ShouldBeAbleToLogin()
        {
            var chrimeDriverDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Driver = new ChromeDriver(chrimeDriverDirectory);
            Driver.Navigate().GoToUrl("https://offishopp.jackslater.ir/login");

            var loginButtonLocator = By.Id("submitLoginButton");
            //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(13));
            //wait.Until(ExpectedConditions.ElementIsVisible(loginButtonLocator));

            var userNameFirld = Driver.FindElement(By.Id("EmailOrPhoneNumber"));
            var passwordField = Driver.FindElement(By.Id("Password"));
            var loginButton = Driver.FindElement(loginButtonLocator);

            userNameFirld.SendKeys("Sinjul.MSBH@Yahoo.Com");
            passwordField.SendKeys("Pa$");
            loginButton.Click();

            passwordField.SendKeys("Pa$$w0rd");
            loginButton.Click();

            Assert.Contains("https://offishopp.jackslater.ir/login?returnUrl=%2F", Driver.Url);
        }

    }
}
