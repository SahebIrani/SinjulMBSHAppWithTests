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

            Person person = new Person { Age = 28, AccountNumber = "444-8547689211-26" };

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

            var person = new Person { Age = 22 };

            await PeopleController.Create(person);

            MockPersonRepository.Verify(x => x.CreatePersonAsync(It.IsAny<Person>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task Create_ModelStateValid_CreatePersonCalledOnceAsync()
        {
            Person ps = null;
            MockPersonRepository.Setup(r => r.CreatePersonAsync(It.IsAny<Person>(), CancellationToken.None))
                .Callback<Person>(x => ps = x);

            var person = new Person { Name = "Sinjul MSBH", Age = 31, AccountNumber = "123-5435789603-21" };

            await PeopleController.Create(person);

            MockPersonRepository.Verify(x => x.CreatePersonAsync(It.IsAny<Person>(), CancellationToken.None), Times.Once);

            Assert.Equal(ps.Name, person.Name);
            Assert.Equal(ps.Age, person.Age);
            Assert.Equal(ps.AccountNumber, person.AccountNumber);
        }

        [Fact]
        public async Task Create_ActionExecuted_RedirectsToIndexActionAsync()
        {
            Person person = new Person { Name = "Sinjul MSBH", Age = 27, AccountNumber = "123-4356874310-43" };

            IActionResult result = await PeopleController.Create(person);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            //RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
