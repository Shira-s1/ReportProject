using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;

namespace ReportProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmployeeService _employeeService;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmployeeLoginRequestDto model)
        {
            _logger.LogInformation($"Login attempt for user: {model?.UserName}");

            if (string.IsNullOrEmpty(model?.UserName) || string.IsNullOrEmpty(model?.Password))
            {
                return BadRequest("Username and password are required.");
            }

            try
            {
                _logger.LogInformation($"Attempting to authenticate user: {model.UserName}");
                var employee = await _employeeService.AuthenticateAsync(model.UserName, model.Password);

                if (employee == null)
                {
                    _logger.LogWarning($"Authentication failed for user: {model.UserName}");
                    return Unauthorized("Invalid username or password.");
                }

                _logger.LogInformation($"User {model.UserName} authenticated successfully.");
                var token = GenerateToken(employee);
                _logger.LogInformation($"JWT token generated for user: {model.UserName}");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during login for user: {model?.UserName}");
                return StatusCode(500, $"Internal server error during login. Exception: {ex.Message}");
            }
        }
        private string GenerateToken(Employee employee)
        {
            var secretKey = _configuration["JWT:key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
             new Claim(JwtRegisteredClaimNames.Sub, employee.UserName),
             new Claim(ClaimTypes.Role, employee.Status.ToString()),
             new Claim("EmployeeId", employee.Id.ToString())
            };


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:ExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class EmployeeLoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}