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
    /// <exception cref="PersonAlreadyExistsException">Выбрасывается, если человек с таким именем уже существует.</exception>
    Task<CorePersone> CreatePersonAsync(CorePersone person);

    /// <summary>
    /// Обновляет данные существующего человека.
    /// </summary>
    /// <param name="id">Идентификатор человека для обновления.</param>
    /// <param name="name">Новое имя.</param>
    /// <param name="age">Новый возраст.</param>
    /// <param name="address">Новый адрес.</param>
    /// <param name="workplace">Новое место работы.</param>
    /// <returns>Обновленный объект человека.</returns>
    /// <exception cref="PersonNotFoundException">Выбрасывается, если человек с указанным ID не найден.</exception>
    Task<CorePersone> UpdatePersonAsync(Guid id, string? name, int? age, string? address, string? workplace);

    /// <summary>
    /// Удаляет человека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор человека для удаления.</param>
    /// <exception cref="PersonNotFoundException">Выбрасывается, если человек с указанным ID не найден.</exception>
    Task DeletePersonByIdAsync(Guid id);

    /// <summary>
    /// Получает человека по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор человека.</param>
    /// <returns>Найденный объект человека.</returns>
    /// <exception cref="PersonNotFoundException">Выбрасывается, если человек с указанным ID не найден.</exception>
    Task<CorePersone> GetPersonByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех людей.
    /// </summary>
    /// <returns>Список всех людей.</returns>
    Task<List<CorePersone>> GetPeopleAsync();
}
