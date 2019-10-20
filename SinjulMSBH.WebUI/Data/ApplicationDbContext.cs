using System;

using Microsoft.EntityFrameworkCore;

using SinjulMSBH.WebUI.Models;

namespace SinjulMSBH.WebUI.Data
{
    public class ApplicationDbContext : /*Identity*/DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasData(
                    new Person { Id = Guid.NewGuid(), Name = "SinjulMSBH", AccountNumber = "123-6482123543-40", Age = 27 },
                    new Person { Id = Guid.NewGuid(), Name = "JackSlater", AccountNumber = "124-6488413682-55", Age = 28 }
                );
        }
    }
}
