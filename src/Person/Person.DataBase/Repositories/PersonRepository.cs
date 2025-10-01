using Microsoft.EntityFrameworkCore;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using Person.DataBase.Context;
using Person.DataBase.Converters;
using CorePerson = Person.Core.Models.Person;
using DataBasePerson = Person.Database.Models.Person;

namespace Person.DataBase.Repositories;

public class PersonRepository : IPersonRepository
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

        var dbPerson = new DataBasePerson(person.Id, person.Name, person.Age, person.Address, person.Work);
        
        await _context.Persons.AddAsync(dbPerson);
        
        await _context.SaveChangesAsync();
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<CorePerson> UpdatePersonAsync(Guid id, string? name, int? age, string? address, string? work)
    {
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (person is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        if (name != person.Name)
        {
            var personWithSameName = await _context.Persons
                .FirstOrDefaultAsync(p => p.Name == name && p.Id != id);
                
            if (personWithSameName != null)
                throw new PersonAlreadyExistsException($"Person with name '{name}' already exists");
        }
        
        if(name != null)
            person.Name = name;
        if(age != null)
            person.Age = age;
        if(address != null)
            person.Address = address;
        if(work != null)
            person.Work = work;
        
        await _context.SaveChangesAsync();
        return PersonConverter.Convert(person);
    }

    public async Task DeletePersonByIdAsync(Guid id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        _context.Persons.Remove(dbPerson);
        await _context.SaveChangesAsync();
    }

    public async Task<CorePerson> GetPersonByIdAsync(Guid id)
    {
        var dbPerson = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (dbPerson is null)
            throw new PersonNotFoundException($"Person with id {id} was not found");
        
        return PersonConverter.Convert(dbPerson);
    }

    public async Task<List<CorePerson>> GetPeopleAsync()
    {
        var persons = await _context.Persons.ToListAsync();
        return persons.ConvertAll(PersonConverter.Convert);
    }
}