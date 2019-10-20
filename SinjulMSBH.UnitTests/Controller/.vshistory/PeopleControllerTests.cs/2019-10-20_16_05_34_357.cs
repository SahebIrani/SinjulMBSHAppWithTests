using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Moq;

using SinjulMSBH.WebUI.Contracts;
using SinjulMSBH.WebUI.Models;

using Xunit;

namespace SinjulMSBH.UnitTests.Controller
{
    public class PeopleControllerTests
    {
        private readonly Mock<IPersonRepository> MockPersonRepository;
        private readonly PeopleController PeopleController;

        public PeopleControllerTests()
        {
            MockPersonRepository = new Mock<IPersonRepository>();
            PeopleController = new PeopleController(MockPersonRepository.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = PeopleController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfPeople()
        {
            MockPersonRepository.Setup(repo => repo.GetAllAsync())
                .Returns(new List<Person>() { new Person(), new Person() });

            var result = PeopleController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var People = Assert.IsType<List<Employee>>(viewResult.Model);
            Assert.Equal(2, People.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = PeopleController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 25, AccountNumber = "255-8547963214-41" };

            var result = PeopleController.Create(employee);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<Employee>(viewResult.Model);
            Assert.Equal(employee.AccountNumber, testEmployee.AccountNumber);
            Assert.Equal(employee.Age, testEmployee.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 34 };

            PeopleController.Create(employee);

            MockPersonRepository.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public void Create_ModelStateValid_CreateEmployeeCalledOnce()
        {
            Employee emp = null;
            MockPersonRepository.Setup(r => r.CreateEmployee(It.IsAny<Employee>()))
                .Callback<Employee>(x => emp = x);

            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 32,
                AccountNumber = "123-5435789603-21"
            };

            PeopleController.Create(employee);

            MockPersonRepository.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Once);

            Assert.Equal(emp.Name, employee.Name);
            Assert.Equal(emp.Age, employee.Age);
            Assert.Equal(emp.AccountNumber, employee.AccountNumber);
        }

        [Fact]
        public void Create_ActionExecuted_RedirectsToIndexAction()
        {
            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 45,
                AccountNumber = "123-4356874310-43"
            };

            var result = PeopleController.Create(employee);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
