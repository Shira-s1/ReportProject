using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;
using ReportProject.Service.Service;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportService reportService, IMapper mapper, ILogger<ReportController> logger)
        {
            _reportService = reportService;
            _mapper = mapper;
            _logger = logger;
        }
        
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<List<ReportWithEmployeeIdDTO>>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all reports");
                var reports = await _reportService.GetAsync();
                var reportDtosWithId = _mapper.Map<List<ReportWithEmployeeIdDTO>>(reports);
                return Ok(reportDtosWithId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all reports", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee, Manager")]
        public async Task<ActionResult<ReportDTO>> Get(int id)
        {
            _logger.LogInformation($"Request received: GET /api/Report/{id}");
            try
            {
                var loggedInUserId = User.FindFirstValue("EmployeeId");
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                var reportEntity = await _reportService.GetReportByIdAsync(id); // שיטה חדשה בסרוויס
                _logger.LogInformation($"Logged in EmployeeId: {loggedInUserId}");
                _logger.LogInformation($"Report EmployeeId: {reportEntity?.Employee?.Id}");
                _logger.LogInformation($"User Role: {userRole}");
                if (reportEntity == null)
                {
                    return NotFound();
                }

                if (userRole == "Employee" && reportEntity.Employee?.Id.ToString() != loggedInUserId)
                {
                    _logger.LogWarning($"Employee with ID {loggedInUserId} tried to access report with ID {id} belonging to another employee. Forbidden.");
                    return Forbid(authenticationSchemes: JwtBearerDefaults.AuthenticationScheme);
                }

                return Ok(_mapper.Map<ReportDTO>(reportEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting report with id {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }




        [HttpPost]
        public async Task<ActionResult<ReportDTO>> Post(int empId, [FromBody] ReportDTO reportDto)
        {
            _logger.LogInformation($"Request received: POST /api/employees/{empId}/reports");

            try
            {
                if (reportDto == null) return BadRequest("Report object cannot be null.");
                var report = _mapper.Map<Report>(reportDto);

              
                var loggedInUserId = User.FindFirstValue("EmployeeId"); // שימוש ב-Claim הספציפי
                _logger.LogInformation($"Logged in user ID (EmployeeId Claim): {loggedInUserId}");

               
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                _logger.LogInformation($"Logged in user role: {userRole}");

                _logger.LogInformation($"empId from URL: {empId}");

                //  הרשאות
                if (userRole == "Employee")
                {
                    if (loggedInUserId != empId.ToString())
                    {
                        _logger.LogWarning($"Employee with ID {loggedInUserId} tried to create report for employee ID {empId}. Forbidden.");
                        return Forbid(authenticationSchemes: JwtBearerDefaults.AuthenticationScheme);
                    }
                }
              
                var createdReport = await _reportService.PostAsync(empId, report);
                var createdReportDto = _mapper.Map<ReportDTO>(createdReport);
                return CreatedAtAction(nameof(Get), new { id = createdReportDto.ReportId }, createdReportDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error creating report for employee ID {empId}: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating report for employee ID {empId}...");
                return StatusCode(500, "An error occurred while creating the report.");
            }
        }

        // PUT api/<ReportController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> Put(int id, [FromBody] ReportDTO reportDto)
        {
            _logger.LogInformation($"Request received: PUT /api/Report/{id}");

            try
            {
                await _reportService.PutAsync(id, reportDto);
                _logger.LogInformation($"Change in {id} report.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Attempted to update non-existent report with ID {id}.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating report with id :{id}");
                return NotFound(ex.Message);
            }
        }


        // DELETE api/<ReportController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Request received: DELETE /api/Report/{id}");
            try
            {
                await _reportService.DeleteAsync(id);
                _logger.LogInformation($"Successfully deleted report with ID {id}.");
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning($"Attempted to delete non-existent report with ID {id}.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting report with id :{id}");
                return StatusCode(500, "An error occurred while deleting the report.");
            }
        }
    }
}
