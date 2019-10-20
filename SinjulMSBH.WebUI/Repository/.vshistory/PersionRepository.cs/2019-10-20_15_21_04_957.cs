using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using SinjulMSBH.WebUI.Contracts;
using SinjulMSBH.WebUI.Data;
using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Repository
{
    public class PersionRepository : IPersonRepository
    {
        public PersionRepository(ApplicationDbContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
        public ApplicationDbContext Context { get; }

        public Task<Person> GetPersonAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await Context.People.AsNoTracking().ToListAsync(cancellationToken);

        public Task CreateEmployee(Person person, CancellationToken cancellationToken = default)
        {
            return await Context.People.AsNoTracking().ToListAsync(cancellationToken);
        }


    }
}
