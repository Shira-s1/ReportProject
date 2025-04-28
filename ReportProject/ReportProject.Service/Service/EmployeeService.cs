using AutoMapper;
using ReportProject.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using ReportProject.Core.Entities;
using ReportProject.Core.Enum;
using ReportProject.Core.Interfaces;
using ReportProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity; 
namespace ReportProject.Service.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IPasswordHasher<Employee> _passwordHasher; // הוספת PasswordHasher
        public EmployeeService(DataContext dataContext, IMapper mapper, ILogger<EmployeeService> logger, IPasswordHasher<Employee> passwordHasher)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }
        public async Task<List<EmployeeGetDTO>> GetAsync()
        {
            _logger.LogInformation("Starting to retrieve all employees.");
            var employees = await _dataContext.empList
                .AsNoTracking()
                .Include(e => e.reportLst)
                .ToListAsync();
            _logger.LogInformation($"Successfully retrieved {employees.Count} employees from the database.");
            return _mapper.Map<List<EmployeeGetDTO>>(employees);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation($"Attempting to retrieve employee entity with ID: {id} including reports");
            var employeeEntity = await _dataContext.empList
                .Include(e => e.reportLst) // **הוסף Include כדי לטעון את רשימת הדיווחים**
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employeeEntity == null)
            {
                _logger.LogWarning($"Employee entity with ID: {id} not found in the database.");
                return null;
            }
            _logger.LogInformation($"Successfully retrieved employee entity with ID: {id} including reports.");
            return employeeEntity;
        }
       
        public async Task<Employee> GetEmployeeByUserNameAsync(string userName)
        {
            return await _dataContext.empList.FirstOrDefaultAsync(e => e.UserName == userName);
        }

        public async Task<Employee> PostEmployeeAsync(Employee employee)
        {
            _logger.LogInformation($"Attempting to add new employee: {employee.FirstName} {employee.LastName}");
            _dataContext.empList.Add(employee);
            await _dataContext.SaveChangesAsync();
            _logger.LogInformation($"Successfully added employee with ID: {employee.Id}");
            return employee;
        }

        public async Task PutEmployeeAsync(Employee employee) // שם הפונקציה שונה
        {
            _logger.LogInformation($"Attempting to update employee with ID: {employee.Id}, Name: {employee.FirstName} {employee.LastName}");
            _dataContext.empList.Update(employee);
            await _dataContext.SaveChangesAsync();
            _logger.LogInformation($"Successfully updated employee with ID: {employee.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting to delete employee with ID {id}.");
            var employee = await _dataContext.empList.FindAsync(id);
            if (employee != null)
            {
                _dataContext.empList.Remove(employee);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"Successfully deleted employee with ID {id} from the database.");
            }
            else
            {
                _logger.LogWarning($"Attempted to delete non-existent employee with ID {id}.");
                throw new KeyNotFoundException($"Employee with id {id} not found");
            }
        }

        public async Task<Employee> AuthenticateAsync(string userName, string password)
        {
            _logger.LogInformation("Attempting to authenticate user: {UserName}", userName);

            var employee = await _dataContext.empList.FirstOrDefaultAsync(e => e.UserName == userName);

            if (employee == null)
            {
                _logger.LogWarning("Authentication failed: User '{UserName}' not found.", userName);
                return null;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                _logger.LogInformation("Authentication successful for user: {UserName}", userName);
                return employee;
            }

            // אם האימות נכשל, נחזיר null בלי לוג מפורש נוסף על סיסמה שגויה
            return null;
        }
    }
}
