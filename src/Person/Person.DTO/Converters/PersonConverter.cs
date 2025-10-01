using Person.DTO.Models;
using CorePerson = Person.Core.Models.Person;

namespace Person.DTO.Converters;

public class PersonConverter
{
    public static CorePerson Convert(PersonResponse person)
    {
        return new CorePerson(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.Work);
    }
    
    public static PersonResponse Convert(CorePerson person)
    {
        return new PersonResponse(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.Work);
    }
    
    public static CorePerson Convert(PersonRequest person)
    {
        return new CorePerson(Guid.NewGuid(),
            person.Name,
            person.Age,
            person.Address,
            person.Work);
    }
    
}