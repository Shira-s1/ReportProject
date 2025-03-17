using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.DTOs;
using ReportProject.Core.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService; 
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/<@try>
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDTO>>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all employees");
                var employees = _employeeService.GetAsync();
                //If you have DTOs, use AutoMapper here
                var employeeDtos = _mapper.Map<List<EmployeeDTO>>(employees);
                return Ok(employees); // Or Ok(employeeDtos)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<@try>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> Get(int id)
        {
            try
            {
                var employee = await _employeeService.GetAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting employee with id {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<@try>
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> Post([FromBody] EmployeeDTO employeeDto)
        {
            try
            {
                await _employeeService.PostAsync(employeeDto);
                return CreatedAtAction(nameof(Get), new { id = employeeDto.Id }, employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<@try>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeDTO employeeDto)
        {
            try
            {
                await _employeeService.PutAsync(id, employeeDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating employee with id {id}");
                return NotFound(ex.Message);
            }
        }

        // DELETE api/<@try>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting employee with id {id}");
                return NotFound(ex.Message);
            }
        }
    }
}
