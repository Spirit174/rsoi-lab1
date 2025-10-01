using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Person.Core.Exceptions;
using Person.Core.Interfaces;
using Person.DTO.Converters;
using Person.DTO.Models;
using Swashbuckle.AspNetCore.Annotations;
using ValidationException = FluentValidation.ValidationException;

using PersonDtoConverter = Person.DTO.Converters.PersonConverter;

namespace Person.Server.Controllers;

[ApiController]
[Route("/api/v1/persons")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private readonly IPersonService _personService;
    private readonly IValidator<PersonRequest> _validator;
    
    public PersonController(ILogger<PersonController> logger,
        IPersonService personService,
        IValidator<PersonRequest> validator)
    {
        _logger = logger;
        _personService = personService;
        _validator = validator;
    }

    /// <summary>
    /// Метод для создания сущности Person.
    /// </summary>
    /// <remarks>Метод для создания сущности Person.</remarks>
    /// <param name="personRequest">Запрос на создание.</param>
    /// <response code="201">Сущность Person успешно создана.</response>
    /// <response code="400">Одно или несколько полей модели невалидны.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPost]
    [SwaggerOperation("Метод для создания сущности Person.", "Метод для создания сущности Person.")]
    [SwaggerResponse(statusCode: 201, description: "Сущность Person успешно создана.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Одно или несколько полей невалидны.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> CreatePersonAsync([Required] [FromBody] PersonRequest personRequest)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(personRequest);

            var personModel = PersonDtoConverter.Convert(personRequest);
            
            var person = await _personService.CreatePersonAsync(personModel);

            var dtoPerson = PersonDtoConverter.Convert(person);

            return Created($"/api/v1/persons/{person.Id}", dtoPerson);
        }
        catch (ValidationException e)
        {
            _logger.LogWarning(e, "Validation failed for person request: {@Model}", personRequest);
            return StatusCode(400, e.ToValidationErrorResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request! {@Request}", personRequest);

            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Метод для получения коллекции сущностей Person.
    /// </summary>
    /// <remarks>Метод для получения коллекции сущностей Person.</remarks>
    /// <response code="200">Коллекция успешно получена.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet]
    [SwaggerOperation("Метод для получения коллекции сущностей Person.", "Метод для получения коллекции сущностей Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(List<PersonResponse>), description: "Коллекция успешно получена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPersonsAsync()
    {
        try
        {
            var people = await _personService.GetPeopleAsync();
            
            var dtoPeople = people.ConvertAll(PersonConverter.Convert);
            
            return Ok(dtoPeople);
        }       
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Метод для получения сущности Person по <paramref name="id"/>
    /// </summary>
    /// <remarks>Метод для получения сущности Person.</remarks>
    /// <param name="id">Идентификатор сущности.</param>
    /// <response code="200">Сущность успешно получена.</response>
    /// <response code="404">Сущность с указанным идентификатором не найдена.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpGet("{id:guid}")]
    [SwaggerOperation("Метод для получения сущности Person.", "Метод для получения сущности Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(PersonResponse), description: "Коллекция успешно получена.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не найдена.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> GetPersonByIdAsync([FromRoute] Guid personId)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(personId);
            
            var dtoPerson = PersonConverter.Convert(person);
            
            return Ok(dtoPerson);
        }       
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {PersonId} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Метод для обновления сущности Person.
    /// </summary>
    /// <remarks>Метод для обновления сущности Person.</remarks>
    /// <param name="personRequest">Запрос на обновление.</param>
    /// <param name="id">Идентификатор сущности.</param>
    /// <response code="200">Сущность Person успешно обновлена.</response>
    /// <response code="400">Одно или несколько полей модели невалидны.</response>
    /// <response code="404">Сущность с указанным идентификатором не существует.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpPatch("{id:guid}")]
    [SwaggerOperation("Метод для обновления сущности Person.", "Метод для обновления сущности Person.")]
    [SwaggerResponse(statusCode: 200, type: typeof(PersonResponse), description: "Сущность Person успешно обновлена.")]
    [SwaggerResponse(statusCode: 400, type: typeof(ErrorResponse), description: "Одно или несколько полей модели невалидны.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не существует.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> UpdatePersonByIdAsync([FromRoute] Guid personId,
        [Required][FromBody] PersonRequest personRequest
        )
    {
        try
        {
            await _validator.ValidateAndThrowAsync(personRequest);
            
            var personModel = PersonDtoConverter.Convert(personRequest);
            personModel.Id = personId;
            
            var person = await _personService.UpdatePersonAsync(personModel);
            
            var dtoPerson = PersonConverter.Convert(person);
            
            return Ok(dtoPerson);
        }
        catch (ValidationException e)
        {
            _logger.LogWarning(e, "One or more field is invalid. {@Model}", personRequest);

            return StatusCode(400, e.ToValidationErrorResponse());
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {PersonId} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request! {@Request}", personRequest);
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
    
    /// <summary>
    /// Метод для удаления сущности Person.
    /// </summary>
    /// <remarks>Метод для удаления сущности Person.</remarks>
    /// <param name="personId">Идентификатор сущности.</param>
    /// <response code="204">Сущность Person успешно удалена.</response>
    /// <response code="404">Сущность с указанным идентификатором не существует.</response>
    /// <response code="500">Ошибка на стороне сервера.</response>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation("Метод для удаления сущности Person.", "Метод для удаления сущности Person.")]
    [SwaggerResponse(statusCode: 204, description: "Сущность Person успешно удалена.")]
    [SwaggerResponse(statusCode: 404, type: typeof(ErrorResponse), description: "Сущность с указанным идентификатором не существует.")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера.")]
    public async Task<IActionResult> DeletePersonByIdAsync([FromRoute] Guid personId)
    {
        try
        {
            await _personService.DeletePersonByIdAsync(personId);

            return StatusCode(204);
        }
        catch (PersonNotFoundException e)
        {
            _logger.LogWarning(e, "Person with id {PersonId} is not found", personId);

            return StatusCode(404, new ErrorResponse($"Сущность Person с идентификатором {personId} не найдена."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected exception while processing request!");
            
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}