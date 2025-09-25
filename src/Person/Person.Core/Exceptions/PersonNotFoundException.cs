namespace Person.Core.Exceptions;

public class PersonNotFoundException : Exception
{
    public PersonNotFoundException()
    {

    }

    public PersonNotFoundException(string? message) : base(message)
    {

    }
    
}