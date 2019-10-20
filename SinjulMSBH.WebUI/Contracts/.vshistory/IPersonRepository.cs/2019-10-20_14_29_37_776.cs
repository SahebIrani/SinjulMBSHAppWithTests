using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Contracts
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default);
        Employee GetEmployee(Guid id);
        void CreateEmployee(Employee employee);
    }
}
