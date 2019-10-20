using System.Collections.Generic;
using System.Threading.Tasks;

using EmployeesApp.Controllers;

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
        public async Task Index_ActionExecutes_ReturnsViewForIndexAsync()
        {
            IActionResult result = await PeopleController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ActionExecutes_ReturnsExactNumberOfPeopleAsync()
        {
            MockPersonRepository.Setup(repo => await repo.GetAllAsync())
                .Returns(new List<Person>() { new Person(), new Person() });

            IActionResult result = await PeopleController.Index();

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            List<Person> People = Assert.IsType<List<Person>>(viewResult.Model);
            Assert.Equal(2, People.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            IActionResult result = PeopleController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required");

            var Person = new Person { Age = 25, AccountNumber = "255-8547963214-41" };

            var result = PeopleController.Create(Person);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testPerson = Assert.IsType<Person>(viewResult.Model);
            Assert.Equal(Person.AccountNumber, testPerson.AccountNumber);
            Assert.Equal(Person.Age, testPerson.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreatePersonNeverExecutes()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required");

            var Person = new Person { Age = 34 };

            PeopleController.Create(Person);

            MockPersonRepository.Verify(x => x.CreatePerson(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void Create_ModelStateValid_CreatePersonCalledOnce()
        {
            Person emp = null;
            MockPersonRepository.Setup(r => r.CreatePerson(It.IsAny<Person>()))
                .Callback<Person>(x => emp = x);

            var Person = new Person
            {
                Name = "Test Person",
                Age = 32,
                AccountNumber = "123-5435789603-21"
            };

            PeopleController.Create(Person);

            MockPersonRepository.Verify(x => x.CreatePerson(It.IsAny<Person>()), Times.Once);

            Assert.Equal(emp.Name, Person.Name);
            Assert.Equal(emp.Age, Person.Age);
            Assert.Equal(emp.AccountNumber, Person.AccountNumber);
        }

        [Fact]
        public void Create_ActionExecuted_RedirectsToIndexAction()
        {
            var Person = new Person
            {
                Name = "Test Person",
                Age = 45,
                AccountNumber = "123-4356874310-43"
            };

            var result = PeopleController.Create(Person);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
