using Microsoft.EntityFrameworkCore;
using Person.DataBase.Context.Configurations;

namespace Person.DataBase.Context;

public class PersonContext(DbContextOptions<PersonContext> options) : DbContext(options)
{
    public DbSet<Database.Models.Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
    }
}