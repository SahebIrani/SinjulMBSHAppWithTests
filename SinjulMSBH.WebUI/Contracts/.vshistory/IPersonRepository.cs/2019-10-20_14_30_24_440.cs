using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Contracts
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default);
        void CreateEmployee(Employee employee);
    }
}
