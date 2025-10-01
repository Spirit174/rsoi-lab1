using Microsoft.Extensions.Logging;
using Person.Core.Exceptions;
using Person.Core.Interfaces;

using CorePerson = Person.Core.Models.Person;

namespace Person.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonService> _logger;
    
    public PersonService(IPersonRepository personRepository,
        ILogger<PersonService> logger)
    {
        _personRepository = personRepository;
        _logger = logger;
    }

    public async Task<CorePerson> CreatePersonAsync(CorePerson person)
    {
        _logger.LogDebug("Creating person with id - {Id}", person.Id);
        try
        {
            var res = await _personRepository.CreatePersonAsync(person);
            _logger.LogInformation("Successfully created person with id - {Id}", res.Id);
            return res;
        }
        catch (PersonAlreadyExistsException e)
        {
            _logger.LogWarning(e, "Failed to create person with id - {Id}", person.Id);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating person with id - {Id}", person.Id);
            throw;
        }
    }
    
    public async Task<CorePerson> UpdatePersonAsync(CorePerson person)
    {
        _logger.LogDebug("Updating person with id - {Id}", person.Id);
        
        try
        {
            var res = await _personRepository.UpdatePersonAsync(person.Id, person.Name, person.Age, person.Address,
                person.Work);
            _logger.LogInformation("Successfully updated person with id {Id}", res.Id);
            return res;
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e,"Person not found during update person with id - {Id}", person.Id);
            throw;
        }
        catch (PersonAlreadyExistsException e)
        {
            _logger.LogWarning(e, "Person already exists during update person with id - {Id}", person.Id);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating person with id - {Id}", person.Id);
            throw;
        }
    }
    
    public async Task<CorePerson> GetPersonByIdAsync(int id)
    {
        _logger.LogDebug("Getting person with id: {Id}", id);
        
        try
        {
            return await _personRepository.GetPersonByIdAsync(id);
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person not found with id - {Id}", id);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting person with id - {Id}", id);
            throw;
        }
    }
    
    public async Task DeletePersonByIdAsync(int id)
    {
        _logger.LogDebug("Deleting person with id - {Id}", id);
        
        try
        {
            await _personRepository.DeletePersonByIdAsync(id);
            _logger.LogInformation("Successfully deleted person with id - {Id}", id);
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e,"Person not found during deletion person with id - {Id}", id);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting person with id - {Id}", id);
            throw;
        }
    }
    
    public async Task<List<CorePerson>> GetPeopleAsync()
    {
        _logger.LogDebug("Getting all people");
        
        try
        {
            var people = await _personRepository.GetPeopleAsync();
            _logger.LogInformation("Retrieved {Count} people", people.Count);
            return people;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all people");
            throw;
        }
    }
}