using System.Collections.Generic;
using System.Threading;
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
            MockPersonRepository.Setup(repo => repo.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(new List<Person>() { new Person(), new Person() });

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
        public async Task Create_InvalidModelState_ReturnsViewAsync()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required");

            Person person = new Person { Age = 25, AccountNumber = "255-8547963214-41" };

            IActionResult result = await PeopleController.Create(person);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testPerson = Assert.IsType<Person>(viewResult.Model);
            Assert.Equal(person.AccountNumber, testPerson.AccountNumber);
            Assert.Equal(person.Age, testPerson.Age);
        }

        [Fact]
        public async Task Create_InvalidModelState_CreatePersonNeverExecutesAsync()
        {
            PeopleController.ModelState.AddModelError("Name", "Name is required .. !!!!");

            var person = new Person { Age = 34 };

            await PeopleController.Create(person);

            MockPersonRepository.Verify(x => await x.CreatePersonAsync(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public async Task Create_ModelStateValid_CreatePersonCalledOnceAsync()
        {
            Person emp = null;
            MockPersonRepository.Setup(r => await r.CreatePersonAsync(It.IsAny<Person>()))
                .Callback<Person>(x => emp = x);

            var person = new Person { Name = "Test Person", Age = 32, AccountNumber = "123-5435789603-21" };

            await PeopleController.Create(person);

            MockPersonRepository.Verify(x => await x.CreatePersonAsync(It.IsAny<Person>()), Times.Once);

            Assert.Equal(emp.Name, person.Name);
            Assert.Equal(emp.Age, person.Age);
            Assert.Equal(emp.AccountNumber, person.AccountNumber);
        }

        [Fact]
        public void Create_ActionExecuted_RedirectsToIndexAction()
        {
            Person person = new Person { Name = "Test Person", Age = 45, AccountNumber = "123-4356874310-43" };

            Task<IActionResult> result = PeopleController.Create(person);

            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
