using Microsoft.AspNetCore.Mvc;

namespace Person.Server.Controllers;

[ApiController]
[Route("/api/v1/persons")]
public class PersonController
{
    public ILogger<PersonController> _logger;
    
    public PersonController(ILogger<PersonController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePersonAsync()
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> GetPersonsAsync()
    {
        
    }
    
    [HttpGet]
    [Route("/api/v1/persons/{personId}")]
    public async Task<IActionResult> GetPersonByIdAsync([FromRoute] string personId)
    {
        
    }

    [HttpPatch]
    [Route("/api/v1/persons/{personId}")]
    public async Task<IActionResult> UpdatePersonByIdAsync([FromRoute] string personId)
    {
        
    }

    [HttpDelete]
    [Route("/api/v1/persons/{personId}")]
    public async Task<IActionResult> DeletePersonByIdAsync([FromRoute] string personId)
    {
        
    }
}