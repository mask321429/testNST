using System;
using NST.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NST.DTO;
using NST.Middleware;

namespace NST.Controllers
{
    [ApiController]
    [Route("api/")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personServiceService)
        {
            _personService = personServiceService;
        }
       
        [HttpGet("v1/persons")]
        public async Task<IActionResult> GetPersons()
        {
            try
            {
                var persons = await _personService.GetListPersons();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected server error", error = ex.Message });
            }
        }

        [HttpGet("v1/persons/{id}")]
        public async Task<IActionResult> GetPersonsById([FromRoute]long id)
        {
            try
            {
                var persons = await _personService.GetInfoPersonById(id);
                return Ok(persons);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected server error", error = ex.Message });
            }
        }
        
        [HttpPost("v1/persons")]
        public async Task<IActionResult> GetUserById(PersonDTO person)
        {
            try
            {
                await _personService.CreateNewPerson(person);
                return Ok("User added to database");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadDataException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Unexpected server error", error = ex.Message });
            }
        }

        
        [HttpPut("v1/person/{id}")]
        public async Task<IActionResult> UpdatePerson([FromRoute]long id, PersonDTO personDto)
        {
            try
            {
                await _personService.UpdatePerson(id, personDto);
                return Ok($"Person with ID {id} has been updated");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadDataException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected server error", error = ex.Message });
            }
        }

        [HttpDelete("v1/person/{id}")]
        public async Task<IActionResult> DeletePerson([FromRoute] long id)
        {
            try
            {
                await _personService.DeletePerson(id);
                return Ok($"Person with ID {id} has been deleted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected server error", error = ex.Message });
            }
        }
        
    }
}


