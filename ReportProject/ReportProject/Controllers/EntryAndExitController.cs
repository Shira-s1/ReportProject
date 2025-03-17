using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;
using ReportProject.Service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryAndExitController : ControllerBase
    {
        private readonly IEnryAndExitService _entryAndExitService;
        public EntryAndExitController(IEnryAndExitService entryAndExitService)
        {
            _entryAndExitService = entryAndExitService;
        }


        // GET: api/<EntryAndExitController>
        [HttpGet]
        public ActionResult<IEnumerable<EntryAndExit>> Get()
        {
            try
            {
                var entriesAndExits = _entryAndExitService.GetAsync();
                return Ok(entriesAndExits); // מחזיר 200 OK עם הרשימה
            }
            catch
            {
                return StatusCode(500, "Error retrieving entries and exits."); // מחזיר 500
            }
        }

        // GET api/<EntryAndExitController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<EntryAndExit>>> Get(int id)
        {
            try
            {
                // קריאה לפונקציה GetAsync בשירות שלך עם ה-ID של העובד
                var entriesAndExits = await _entryAndExitService.GetAsync(id);

                if (entriesAndExits == null)
                {
                    return NotFound($"No entries and exits found for employee with ID {id}");
                }

                return Ok(entriesAndExits);
            }
            catch (Exception ex)
            {
                // לוג את השגיאה
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // POST api/<EntryAndExitController>
        [HttpPost]
        public async Task<ActionResult<EntryAndExit>> Post([FromBody] EntryAndExit entryAndExit)
        {
            try
            {
                await _entryAndExitService.PostAsync(entryAndExit);
                return CreatedAtAction(nameof(Get), new { id = entryAndExit.Id }, entryAndExit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<EntryAndExitController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EntryAndExit entryAndExit)
        {
            try
            {
                await _entryAndExitService.PutAsync(id, entryAndExit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE api/<EntryAndExitController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _entryAndExitService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
