using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Person.DataBase.Context.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Database.Models.Person>
{
    public void Configure(EntityTypeBuilder<Database.Models.Person> builder)
    {
        builder.HasIndex(person => person.Id).IsUnique();
        builder.HasKey(person => person.Id);

        builder.Property(person => person.Id).IsRequired();

        builder.Property(person => person.Name).IsRequired();
    }
}
