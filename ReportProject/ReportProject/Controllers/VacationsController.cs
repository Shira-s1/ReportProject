using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;
using ReportProject.Service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly IVacationsService _vacationService;
        public VacationsController(IVacationsService vacationService)
        {
            _vacationService = vacationService;
        }

        // GET: api/<VacationsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vacations>>> Get()
        {
            try
            {
                var vacations = await _vacationService.GetAsync();
                return Ok(vacations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET api/<VacationsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Vacations>>> Get(int id)
        {
            try
            {
                var vacations = await _vacationService.GetAsync(id);
                if (vacations == null )
                {
                    return NotFound($"No vacations found for employee ID {id}.");
                }
                return Ok(vacations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST api/<VacationsController>
        [HttpPost]
        public async Task<ActionResult<Vacations>> Post([FromBody] Vacations vacation)
        {
            try
            {
                await _vacationService.PostAsync(vacation);
                return CreatedAtAction(nameof(Get), new { id = vacation.Id }, vacation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<VacationsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Vacations vacation)
        {
            try
            {
                await _vacationService.PutAsync(id, vacation);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/<VacationsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _vacationService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
