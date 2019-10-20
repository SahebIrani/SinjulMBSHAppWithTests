using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Contracts
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Employee GetEmployee(Guid id);
        void CreateEmployee(Employee employee);
    }
}
