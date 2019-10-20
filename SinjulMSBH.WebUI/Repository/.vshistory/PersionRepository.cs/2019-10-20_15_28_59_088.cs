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

        public async Task<Person> GetPersonAsync(Guid id, CancellationToken cancellationToken = default) =>
            await Context.People.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await Context.People.AsNoTracking().ToListAsync(cancellationToken);

        public async Task CreateEmployee(Person person, CancellationToken cancellationToken = default)
        {
            person.Id = person.Id == null ? Guid.NewGuid() : person.Id;
            await Context.AddAsync(person, cancellationToken).ConfigureAwait(false);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
