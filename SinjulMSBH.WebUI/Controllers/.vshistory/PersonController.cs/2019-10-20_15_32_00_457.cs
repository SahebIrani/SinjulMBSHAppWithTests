using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using SinjulMSBH.WebUI.Contracts;
using SinjulMSBH.WebUI.Validation;

namespace EmployeesApp.Controllers
{
    public class PersonController : Controller
    {
        public EmployeesController(IPersonRepository personRepository)
        {
            accountNumberValidation = new AccountNumberValidation();
            PersonRepository = personRepository ?? throw new System.ArgumentNullException(nameof(personRepository));
        }

        private readonly AccountNumberValidation accountNumberValidation;
        public IPersonRepository PersonRepository { get; }

        public IActionResult Index()
        {
            var employees = _repo.GetAll();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,AccountNumber,Age")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (!_validation.IsValid(employee.AccountNumber))
            {
                ModelState.AddModelError("AccountNumber", "Account Number is invalid");
                return View(employee);
            }

            _repo.CreateEmployee(employee);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
