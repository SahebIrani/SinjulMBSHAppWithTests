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
            EmployeePage = new EmployeePage(WebDriver);
            EmployeePage.Navigate();
        }

        private readonly IWebDriver WebDriver;
        private readonly EmployeePage EmployeePage;

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            Assert.Equal("Create - EmployeesApp", EmployeePage.Title);
            Assert.Contains("Please provide a new employee data", EmployeePage.Source);
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            EmployeePage.PopulateName("New Name");
            EmployeePage.PopulateAge("34");
            EmployeePage.ClickCreate();

            Assert.Equal("Account number is required", EmployeePage.AccountNumberErrorMessage);
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewEmployee()
        {
            EmployeePage.PopulateName("New Name");
            EmployeePage.PopulateAge("34");
            EmployeePage.PopulateAccountNumber("123-9384613085-58");
            EmployeePage.ClickCreate();

            Assert.Equal("Index - EmployeesApp", EmployeePage.Title);
            Assert.Contains("New Name", EmployeePage.Source);
            Assert.Contains("34", EmployeePage.Source);
            Assert.Contains("123-9384613085-58", EmployeePage.Source);
        }

        public void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
