using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SinjulMSBH.WebUI.Contracts;
using SinjulMSBH.WebUI.Models;
using SinjulMSBH.WebUI.Validation;

namespace EmployeesApp.Controllers
{
    public class PersonController : Controller
    {
        public PersonController(IPersonRepository personRepository)
        {
            accountNumberValidation = new AccountNumberValidation();
            PersonRepository = personRepository ?? throw new System.ArgumentNullException(nameof(personRepository));
        }

        private readonly AccountNumberValidation accountNumberValidation;
        public IPersonRepository PersonRepository { get; }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            IEnumerable<Person> people = await PersonRepository.GetAllAsync(cancellationToken);
            return View(people);
        }

        public async Task<ActionResult<Person>> GetPerson(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            Person person = await PersonRepository.GetPersonAsync(id, cancellationToken);
            return person;
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,AccountNumber,Age")] Person person,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return View(person);

            if (!accountNumberValidation.IsValid(person.AccountNumber))
            {
                ModelState.AddModelError("AccountNumber", "Account Number is invalid .. !!!!");
                return View(person);
            }

            await PersonRepository.CreatePersonAsync(person, cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
