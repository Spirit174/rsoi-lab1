using Microsoft.EntityFrameworkCore;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using Person.DataBase.Context;
using Person.DataBase.Repositories;
using Xunit;

namespace Person.Tests;

public class UnitTests
{
    private readonly DbContextOptions<PersonContext> _dbContextOptions;
    private readonly PersonContext _context;
    private readonly IPersonRepository _repository;

    public UnitTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<PersonContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PersonContext(_dbContextOptions);
        _repository = new PersonRepository(_context);
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldCreatePerson_WhenValidData()
    {
        // Arrange
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(Guid.NewGuid(), name, age, address, workplace);

        // Act
        var result = await _repository.CreatePersonAsync(person);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
        Assert.Equal(address, result.Address);
        Assert.Equal(workplace, result.WorkPlace);

        // Verify in database
        var dbPerson = await _context.Persons.FirstOrDefaultAsync();
        Assert.NotNull(dbPerson);
        Assert.Equal(name, dbPerson.Name);
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldThrowException_WhenPersonWithSameNameExists()
    {
        // Arrange
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(Guid.NewGuid(), name, age, address, workplace);
        await _repository.CreatePersonAsync(person);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonAlreadyExistsException>(() =>
            _repository.CreatePersonAsync(person));

        Assert.Contains($"Person with name {person.Name} already exists", exception.Message);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);
        var personId = id;

        // Act
        var result = await _repository.GetPersonByIdAsync(personId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(personId, result.Id);
        Assert.Equal("Ya", result.Name);
    }

    [Fact]
    public async Task GetPersonByIdAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.GetPersonByIdAsync(nonExistentId));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdatePerson_WhenValidData()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);

        var newName = "NeYA";
        var newAge = 20;
        var newAddress = "KrasnoKazarmennya1";
        var newWork = "Developer";

        // Act
        var result = await _repository.UpdatePersonAsync(id, newName, newAge, newAddress, newWork);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(newName, result.Name);
        Assert.Equal(newAge, result.Age);
        Assert.Equal(newAddress, result.Address);
        Assert.Equal(newWork, result.WorkPlace);

        // Verify in database
        var updatedPerson = await _repository.GetPersonByIdAsync(id);
        Assert.Equal(newName, updatedPerson.Name);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.UpdatePersonAsync(nonExistentId, "NeYA", 20, "KrasnoKazarmennya1", "Developerv"));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldThrowException_WhenNameConflict()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);
        
        var id1 = Guid.NewGuid();
        var name1 = "NeYA";
        var age1 = 20;
        var address1 = "KrasnoKazarmennya1";
        var workplace1 = "Developer";
        
        var person1 = new Core.Models.Person(id1, name1, age1, address1, workplace1);
        await _repository.CreatePersonAsync(person1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonAlreadyExistsException>(() =>
            _repository.UpdatePersonAsync(id, "NeYA", 22, "Address", "Work"));

        Assert.Contains("Person with name 'NeYA' already exists", exception.Message);
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldAllowSameName_WhenUpdatingSamePerson()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);

        // Act - обновляем с тем же именем (должно работать)
        var result = await _repository.UpdatePersonAsync(id, "Ya", 69, "Address", "Work");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ya", result.Name);
        Assert.Equal(69, result.Age);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldDeletePerson_WhenPersonExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);

        // Act
        await _repository.DeletePersonByIdAsync(id);
        

        // Verify person is deleted
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.GetPersonByIdAsync(id));

        Assert.Contains($"Person with id {id} was not found", exception.Message);
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldThrowException_WhenPersonNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<PersonNotFoundException>(() =>
            _repository.DeletePersonByIdAsync(nonExistentId));

        Assert.Contains($"Person with id {nonExistentId} was not found", exception.Message);
    }

    [Fact]
    public async Task GetPeopleAsync_ShouldReturnAllPersons()
    {
        // Arrange
        // Arrange
        var id = Guid.NewGuid();
        var name = "Ya";
        var age = 21;
        var address = "KrasnoKazarmennya";
        var workplace = "Devops";

        var person = new Core.Models.Person(id, name, age, address, workplace);
        await _repository.CreatePersonAsync(person);
        
        var id1 = Guid.NewGuid();
        var name1 = "NeYA";
        var age1 = 20;
        var address1 = "KrasnoKazarmennya1";
        var workplace1 = "Developer";
        
        var person1 = new Core.Models.Person(id1, name1, age1, address1, workplace1);
        await _repository.CreatePersonAsync(person1);

        // Act
        var result = await _repository.GetPeopleAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "Ya");
        Assert.Contains(result, p => p.Name == "NeYA");
    }

    [Fact]
    public async Task GetPeopleAsync_ShouldReturnEmptyList_WhenNoPersons()
    {
        // Act
        var result = await _repository.GetPeopleAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}