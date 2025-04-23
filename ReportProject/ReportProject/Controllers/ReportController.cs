using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;
using ReportProject.Service.Service;

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
        // GET: api/<ReportController>
        [HttpGet]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<List<ReportDTO>>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all reports");
                var employees = await _reportService.GetAsync();

                //var employeeDtos = _mapper.Map<List<EmployeeDTO>>(employees);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET api/<ReportController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<ActionResult<ReportDTO>> Get(int id)
        {
            _logger.LogInformation($"Request received: GET /api/Report/{id}");
            try
            {
                var employee = await _reportService.GetAsync(id);
                return employee == null ? NotFound() : Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting report with id {id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // POST api/<ReportController>
        [HttpPost]
        public async Task<ActionResult<ReportDTO>> Post([FromBody] ReportDTO reportDto)
        {
            _logger.LogInformation("Request received: Report /api/report");

            try
            {
                if (reportDto == null) return BadRequest("Report object cannot be null.");
                var report = _mapper.Map<Report>(reportDto);
                var createdReport = await _reportService.PostAsync(report);
                var createdReportDto = _mapper.Map<ReportDTO>(createdReport);
                return CreatedAtAction(nameof(Get), new { id = createdReportDto.ReportId }, createdReportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report...");
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ReportController>/5
        [HttpPut("{id}")]
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
