using CorePerson = Person.Core.Models.Person;
using DataBasePerson = Person.Database.Models.Person;

namespace Person.DataBase.Converters;

public class PersonConverter
{
    public static CorePerson Convert(DataBasePerson person)
    {
        return new CorePerson(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.WorkPlace);
    }
    
    public static DataBasePerson Convert(CorePerson person)
    {
        return new DataBasePerson(person.Id,
            person.Name,
            person.Age,
            person.Address,
            person.WorkPlace);
    }
}