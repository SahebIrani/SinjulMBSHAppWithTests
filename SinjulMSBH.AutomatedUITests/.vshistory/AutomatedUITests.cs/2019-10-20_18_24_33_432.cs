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
            personPage = new personPage(WebDriver);
            personPage.Navigate();
        }

        private readonly IWebDriver WebDriver;
        private readonly personPage personPage;

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            Assert.Equal("Create - SinjulMSBH.WebUI", personPage.Title);
            Assert.Contains("Please provide a new person data", personPage.Source);
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            personPage.PopulateName("New Name");
            personPage.PopulateAge("34");
            personPage.ClickCreate();

            Assert.Equal("Account number is required", personPage.AccountNumberErrorMessage);
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewperson()
        {
            personPage.PopulateName("New Name");
            personPage.PopulateAge("34");
            personPage.PopulateAccountNumber("123-9384613085-58");
            personPage.ClickCreate();

            Assert.Equal("Index - personsApp", personPage.Title);
            Assert.Contains("New Name", personPage.Source);
            Assert.Contains("34", personPage.Source);
            Assert.Contains("123-9384613085-58", personPage.Source);
        }

        public void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
