namespace Person.Database.Models;

public class Person
{
    /// <summary>
    /// Идентификатор человека.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Возраст.
    /// </summary>
    public int? Age { get; set; }
    
    /// <summary>
    /// Адрес.
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// Место работы.
    /// </summary>
    public string? Work { get; set; }

    public Person(Guid id,
        string name,
        int? age,
        string? address,
        string? work)
    {
        Id = id;
        Name = name;
        Age = age;
        Address = address;
        Work = work;
    }
}