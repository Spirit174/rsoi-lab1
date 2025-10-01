using CorePerson = Person.Core.Models.Person;

namespace Person.Core.Interfaces;

/// <summary>
/// Сервис для работы с людьми
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Создает новой объект человека
    /// </summary>
    /// <param name="person">Объект человека для создания</param>
    /// <returns>Созданный объект человека</returns>
    Task<CorePerson> CreatePersonAsync(CorePerson person);

    /// <summary>
    /// Обновляет данные существующего человека
    /// </summary>
    /// <param name="person">Объект человека с обновленными данными</param>
    /// <returns>Обновленный объект человека</returns>
    Task<CorePerson> UpdatePersonAsync(CorePerson person);

    /// <summary>
    /// Получает человека по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор человека</param>
    /// <returns>Найденный объект человека</returns>
    Task<CorePerson> GetPersonByIdAsync(int id);

    /// <summary>
    /// Удаляет человека по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор человека для удаления</param>
    Task DeletePersonByIdAsync(int id);

    /// <summary>
    /// Получает список всех людей
    /// </summary>
    /// <returns>Список всех людей</returns>
    Task<List<CorePerson>> GetPeopleAsync();
}