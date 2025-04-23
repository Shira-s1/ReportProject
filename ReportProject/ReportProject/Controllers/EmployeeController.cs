using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using ReportProject.Core.Enum;
using ReportProject.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize]
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
        public async Task<ActionResult<List<EmployeeGetDTO>>> Get()
        {
            _logger.LogInformation("Request received: GET /api/Employee");
            //// --- קוד בדיקה זמני ---
            //try
            //{
            //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "test_write.txt");
            //    System.IO.File.WriteAllText(filePath, "This is a test.");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Error writing test file: {ex.Message}");
            //}
            //// --- סוף קוד בדיקה זמני ---

            try
            {
                _logger.LogInformation("Getting all employees");
                var employeeDtos = await _employeeService.GetAsync(); // הסרוויס כבר מחזיר DTOs
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<@try>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeGetDTO>> Get(int id)
        {
            _logger.LogInformation($"Request received: GET /api/Employee/{id}");
            try
            {
                var employee = await _employeeService.GetAsync(id);
                if (employee == null)
                {
                    _logger.LogWarning($"Employee with ID {id} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Successfully retrieved employee with ID {id}.");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting employee with id {id}");
                return StatusCode(500, "Internal Server Error");
            }

            //בדיקה למידלוואר שמציג שגיאה 
            //_logger.LogInformation($"Request received: GET /api/Employee/{id}");
            //throw new Exception("This is a test exception for the ErrorMiddleware.");
            //var employee = await _employeeService.GetAsync(id);
            //if (employee == null)
            //{
            //    _logger.LogWarning($"Employee with ID {id} not found.");
            //    return NotFound();
            //}
            //_logger.LogInformation($"Successfully retrieved employee with ID {id}.");
            //return Ok(employee);
        }

        //before change------------------------------------
        // POST api/<@try>
        //[HttpPost]//שינוי!!
        ////[Authorize(Policy = "ManagerOnly")]//!!!!!!!!!!!!
        //public async Task<ActionResult<EmployeeDTO>> Post([FromBody] EmployeeDTO employeeDto)
        //{
        //    _logger.LogInformation("Request received: POST /api/Employee");

        //    try
        //    {
        //        if (employeeDto == null) return BadRequest("Employee object cannot be null.");

        //        var employee = _mapper.Map<Employee>(employeeDto);

        //        // אם Id הוא 0, זה עובד חדש - נאתחל את reportLst ל-null כדי שהסרוויס יתעלם
        //        if (employee.Id == 0)
        //        {
        //            employee.reportLst = null; // או new List<Report>() אם אתה רוצה רשימה ריקה
        //        }

        //        var createdEmployee = await _employeeService.PostAsync(employee);
        //        var createdEmployeeDto = _mapper.Map<EmployeeDTO>(createdEmployee);
        //        _logger.LogInformation($"Successfully created employee with ID {createdEmployeeDto.Id}.");
        //        return CreatedAtAction(nameof(Get), new { id = createdEmployeeDto.Id }, createdEmployeeDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating employee");
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpPut("{id}")]
        ////[Authorize(Policy = "ManagerOnly")]!!!!!!!!!!!!!!!!!!!!!!!
        //public async Task<IActionResult> Put(int id, [FromBody] EmployeeDTO employeeDto)
        //{
        //    _logger.LogInformation($"Request received: PUT /api/Employee/{id}");
        //    if (id != employeeDto.Id)
        //    {
        //        return BadRequest("ID mismatch");
        //    }

        //    if (employeeDto == null)
        //    {
        //        _logger.LogWarning($"ID mismatch in PUT request. URL ID: {id}, Body ID: {employeeDto.Id}");
        //        return BadRequest("Employee object cannot be null.");
        //    }


        //    // **פתרון זמני לבעיית הסיסמה ב-PUT:**
        //    // אם הסיסמה ב-DTO לא סופקה (null או ריקה), השתמש בסיסמה ברירת מחדל
        //    //if (string.IsNullOrEmpty(employee.Password))
        //    //{
        //    //    employee.Password = "defaultPassword"; // **החלף בסיסמה ברירת מחדל משלך**
        //    //}
        //    //// באופן דומה, טפל בשם המשתמש אם הוא עלול להיות null
        //    //if (string.IsNullOrEmpty(employee.UserName))
        //    //{
        //    //    employee.UserName = "defaultUserName"; // **החלף בשם משתמש ברירת מחדל משלך**
        //    //}

        //    try
        //    {
        //        var employee = _mapper.Map<Employee>(employeeDto);
        //        await _employeeService.PutAsync(employee);
        //        _logger.LogInformation($"Successfully updated employee with ID {id}.");
        //        return NoContent();
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        _logger.LogWarning($"Attempted to update non-existent employee with ID {id}.");
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error updating employee with ID: {id}");
        //        return StatusCode(500, "An error occurred while updating the employee.");
        //    }
        //}
        //---------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        {
            _logger.LogInformation("Request received: POST /api/Employee");

            try
            {             
                var createdEmployee = await _employeeService.PostAsync(employee);
                return CreatedAtAction(nameof(Get), new { id = createdEmployee.Id }, createdEmployee);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error creating employee - Null argument");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]//הרשאה - רק מנהל יכול להוסיף עובד
        public async Task<IActionResult> Put(int id, [FromBody] Employee employee)
        {
            _logger.LogInformation($"Request received: PUT /api/Employee/{id}");

            if (id != employee.Id)
                return BadRequest("ID mismatch");
            
            if (employee == null)
            {
                _logger.LogWarning($"ID mismatch in PUT request. URL ID: {id}, Body ID: {employee.Id}");
                return BadRequest("Employee object cannot be null.");
            }

            try
            {
                await _employeeService.PutAsync(id, employee);
                _logger.LogInformation($"Successfully updated employee with ID {id}.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Attempted to update non-existent employee with ID {id}.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating employee with ID: {id}");
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }



        //public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        //{
        //    try
        //    {
        //        if (employee == null)
        //        {
        //            return BadRequest("Employee object cannot be null.");
        //        }

        //        var createdEmployee = await _employeeService.PostAsync(employee);
        //        return CreatedAtAction(nameof(Get), new { id = createdEmployee.Id }, createdEmployee);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating employee");
        //        return BadRequest(ex.Message);
        //    }
        //}       
        // PUT api/<@try>/5
        //[HttpPut("{id}")]עובד
        //public async Task<IActionResult> Put(int id, [FromBody] EmployeeDTO employeeDto)
        //{
        //    try
        //    {
        //        await _employeeService.PutAsync(id, employeeDto);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error updating employee with id {id}");
        //        return NotFound(ex.Message);
        //    }
        //}

        // DELETE api/<@try>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]//הרשאה - רק מנהל יכול למחוק עובד
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Request received: DELETE /api/Employee/{id}");
            try
            {
                await _employeeService.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted employee with ID {id}.");
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
