using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using SinjulMSBH.WebUI.Contracts;
using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Repository
{
    public class PersionRepository : IPersonRepository
    {
        public Task CreateEmployee(Person person, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPersonAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
