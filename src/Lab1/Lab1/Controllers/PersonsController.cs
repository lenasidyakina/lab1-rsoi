using Lab1.Dtos;
using Lab1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers;

[ApiController]
[Route("api/v1/persons")]
[Produces("application/json")]
[Tags("Person REST API operations")]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonsController(IPersonService service) => _service = service;

    /// <summary>Get all Persons</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonResponse>>> ListPersons(CancellationToken ct)
    {
        var result = await _service.GetAllAsync(ct);
        return Ok(result);
    }

    /// <summary>Get Person by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> GetPerson(int id, CancellationToken ct)
    {
        var person = await _service.GetByIdAsync(id, ct);
        if (person is null)
            return NotFound(new ErrorResponse($"Person with id {id} not found"));

        return Ok(person);
    }

    /// <summary>Create new Person</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePerson([FromBody] PersonRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetPerson), new { id = created.Id }, null);
    }

    /// <summary>Update Person by ID</summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> UpdatePerson(int id, [FromBody] PersonRequest request, CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, request, ct);
        if (updated is null)
            return NotFound(new ErrorResponse($"Person with id {id} not found"));

        return Ok(updated);
    }

    /// <summary>Remove Person by ID</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerson(int id, CancellationToken ct)
    {
        var deleted = await _service.DeleteAsync(id, ct);
        if (!deleted)
            return NotFound(new ErrorResponse($"Person with id {id} not found"));

        return NoContent();
    }
}