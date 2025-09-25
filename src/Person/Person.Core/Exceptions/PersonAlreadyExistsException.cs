namespace Person.Core.Exceptions;

public class PersonAlreadyExistsException : Exception
{
    public PersonAlreadyExistsException()
    {

    }

    public PersonAlreadyExistsException(string? message) : base(message)
    {

    }
    
}