using System.Collections.Generic;
using System.Diagnostics;
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

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,AccountNumber,Age")] Person person)
        {
            if (!ModelState.IsValid) return View(person);

            if (!accountNumberValidation.IsValid(person.AccountNumber))
            {
                ModelState.AddModelError("AccountNumber", "Account Number is invalid");
                return View(person);
            }

            await PersonRepository.CreatePersonAsync(person, CancellationToken cancellationToken = default);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
