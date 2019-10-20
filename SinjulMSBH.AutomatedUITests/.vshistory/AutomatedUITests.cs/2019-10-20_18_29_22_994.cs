using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Xunit;

namespace SinjulMSBH.AutomatedUITests
{
    public class AutomatedUITests : IDisposable
    {
        public AutomatedUITests()
        {
            WebDriver = new ChromeDriver();
            personPage = new PersonPage(WebDriver);
            personPage.Navigate();
        }

        private readonly IWebDriver WebDriver;
        private readonly PersonPage personPage;

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            Assert.Equal("Create - SinjulMSBH.WebUI", personPage.Title);
            Assert.Contains("Please provide a new person data", personPage.Source);
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            personPage.PopulateName("Sinjul MSBH");
            personPage.PopulateAge("27");
            personPage.ClickCreate();

            Assert.Equal("Account number is required", personPage.AccountNumberErrorMessage);
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewperson()
        {
            personPage.PopulateName("Index - SinjulMSBH.WebUI");
            personPage.PopulateAge("34");
            personPage.PopulateAccountNumber("123-9384613085-58");
            personPage.ClickCreate();

            Assert.Equal("Index - SinjulMSBH.WebUI", personPage.Title);
            Assert.Contains("Jack Slater", personPage.Source);
            Assert.Contains("28", personPage.Source);
            Assert.Contains("123-9384613085-58", personPage.Source);
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    WebDriver.Quit();
                    WebDriver.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose() => Dispose(true);
    }
}
