using OpenQA.Selenium;

namespace SinjulMSBH.AutomatedUITests
{
    public class EmployeePage
    {
        public EmployeePage(IWebDriver driver)
        {
            WebDriver = driver;
        }

        private readonly IWebDriver WebDriver;
        private const string URI = "https://localhost:5001/People/Create";

        private IWebElement NameElement => WebDriver.FindElement(By.Id("Name"));
        private IWebElement AgeElement => WebDriver.FindElement(By.Id("Age"));
        private IWebElement AccountNumberElement => WebDriver.FindElement(By.Id("AccountNumber"));
        private IWebElement CreateElement => WebDriver.FindElement(By.Id("Create"));

        public string Title => WebDriver.Title;
        public string Source => WebDriver.PageSource;
        public string AccountNumberErrorMessage => WebDriver.FindElement(By.Id("AccountNumber-error")).Text;


        public void Navigate() => WebDriver.Navigate()
                .GoToUrl(URI);

        public void PopulateName(string name) => NameElement.SendKeys(name);
        public void PopulateAge(string age) => AgeElement.SendKeys(age);
        public void PopulateAccountNumber(string accountNumber) => AccountNumberElement.SendKeys(accountNumber);
        public void ClickCreate() => CreateElement.Click();
    }
}
