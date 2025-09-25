using Microsoft.EntityFrameworkCore;
using Person.Core.Exceptions;
using Person.DataBase.Context;
using Person.DataBase.Converters;
using CorePerson = Person.Core.Models.Person;
using DataBasePerson = Person.Database.Models.Person;

namespace Person.DataBase.Repositories;

public class PersonRepository
{
    private readonly PersonContext _context;
    
    public PersonRepository(PersonContext context)
    {
        _context = context;
    }

    public async Task<CorePerson> CreatePersonAsync(CorePerson person)
    {
        var existingPerson = await _context.Persons
            .FirstOrDefaultAsync(p => p.Name == person.Name);
            
        if (existingPerson != null)
            throw new PersonAlreadyExistsException($"Person with name {person.Name} already exists");

        var dbPerson = new DataBasePerson(person.Id, person.Name, person.Age, person.Address, person.WorkPlace);
        
        await _context.Persons.AddAsync(dbPerson);
        
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> UpdatePersonAsync(Guid id, string? name, int? age, string? address, string? workplace)
    {
        var person = PersonConverter.Convert(await GetPersonByIdAsync(id));
        
        if(name != null)
            person.Name = name;
        if(age != null)
            person.Age = age;
        if(address != null)
            person.Address = address;
        if(workplace != null)
            person.WorkPlace = workplace;
        
        await _context.SaveChangesAsync();
        return PersonConverter.Convert(person);
    }

    public async Task<CorePerson> DeletePersonByIdAsync(Guid id)
    {
        
    }

    public async Task<CorePerson> GetPersonByIdAsync(Guid id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> GetPeopleAsync(CorePerson person)
    {
        
    }
}