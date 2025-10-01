using CorePersone = Person.Core.Models.Person;

namespace Person.Core.Interfaces;

/// <summary>
/// Репозиторий для работы с данными о людях.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Создает нового человека.
    /// </summary>
    /// <param name="person">Объект человека для создания.</param>
    /// <returns>Созданный объект человека.</returns>
    Task<CorePersone> CreatePersonAsync(CorePersone person);

    /// <summary>
    /// Обновляет данные существующего человека.
    /// </summary>
    /// <param name="id">Идентификатор человека для обновления.</param>
    /// <param name="name">Новое имя.</param>
    /// <param name="age">Новый возраст.</param>
    /// <param name="address">Новый адрес.</param>
    /// <param name="work">Новое место работы.</param>
    /// <returns>Обновленный объект человека.</returns>
    Task<CorePersone> UpdatePersonAsync(Guid id, string? name, int? age, string? address, string? work);

    /// <summary>
    /// Удаляет человека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор человека для удаления.</param>
    Task DeletePersonByIdAsync(Guid id);

    /// <summary>
    /// Получает человека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор человека.</param>
    /// <returns>Найденный объект человека.</returns>
    Task<CorePersone> GetPersonByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех людей.
    /// </summary>
    /// <returns>Список всех людей.</returns>
    Task<List<CorePersone>> GetPeopleAsync();
}
