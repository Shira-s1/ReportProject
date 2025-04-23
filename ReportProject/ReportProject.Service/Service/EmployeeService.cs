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
        public async Task<EmployeeGetDTO> GetAsync(int id)
        {
            _logger.LogInformation($"Attempting to retrieve employee with ID: {id}");
            // var emp = await _dataContext.empList.FirstOrDefaultAsync(e => e.Id == id);
            var employee = await _dataContext.empList
                  .Include(e => e.reportLst)
                  .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                _logger.LogWarning($"Employee with ID {id} not found in the database.");
                return null;
            }
            _logger.LogInformation($"Successfully retrieved employee with ID {id} from the database.");
            return _mapper.Map<EmployeeGetDTO>(employee);
        }
        //public async Task<Employee> PostAsync(Employee employee)//הוספה
        //{
        //    if (employee == null)
        //        throw new Exception("Employee is null");

        //    _dataContext.empList.Add(employee);
        //    await _dataContext.SaveChangesAsync();
        //    return employee; // Return the created entity
        //}
        //public async Task PutAsync(int id, EmployeeDTO employeeDto)
        //{
        //    var existingEmployee = await _dataContext.empList.FindAsync(id);
        //    if (existingEmployee == null)
        //    {
        //        throw new KeyNotFoundException($"Employee with id {id} not found");
        //    }

        //    _mapper.Map(employeeDto, existingEmployee); // מיפוי לעובד קיים
        //    await _dataContext.SaveChangesAsync();
        //}



        //!! עם נתינת ערך דיפולטיבי לססמה ולשם משתמש
        //public async Task<Employee> PostAsync(Employee employee)
        //{
        //    if (employee == null)
        //        throw new Exception("Employee is null");

        //    _dataContext.empList.Add(employee);
        //    await _dataContext.SaveChangesAsync();
        //    return employee;
        //}
        //public async Task<Employee> PostAsync(Employee employee)
        //{
        //    _logger.LogInformation($"Starting to create new employee");
        //    if (employee == null)
        //        throw new Exception("Employee is null");

        //    if (string.IsNullOrEmpty(employee.UserName) || string.IsNullOrEmpty(employee.Password))
        //    {
        //        throw new Exception("Username and password are required for new employees.");
        //    }

        //    employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

        //    _dataContext.empList.Add(employee);
        //    await _dataContext.SaveChangesAsync();
        //    _logger.LogInformation($"Successfully created employee with ID {employee.Id} in the database.");
        //    return employee;
        //}
        //public async Task PutAsync(Employee employee)
        //{
        //    _logger.LogInformation($"Successfully created employee with ID {employee.Id}.");
        //    var existingEmployee = await _dataContext.empList
        //        .Include(e => e.reportLst)
        //        .FirstOrDefaultAsync(e => e.Id == employee.Id);

        //    if (existingEmployee == null)
        //    {
        //        throw new KeyNotFoundException($"Employee with id {employee.Id} not found");
        //    }

        //    // עדכון מאפייני העובד הבסיסיים
        //    _dataContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
        //    // אם הסיסמה סופקה ב-DTO (ובעקבות כך ב-employee), עדכן אותה
        //    if (!string.IsNullOrEmpty(employee.Password) && employee.Password != existingEmployee.Password)
        //    {
        //        // הצפנת הסיסמה החדשה
        //        existingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
        //        _dataContext.Entry(existingEmployee).Property(e => e.Password).IsModified = true;
        //    }

        //    // טיפול ברשימת הדיווחים
        //    var incomingReports = employee.reportLst ?? new List<Report>();
        //    var existingReports = existingEmployee.reportLst.ToList();

        //    // מחיקת דיווחים שהוסרו
        //    foreach (var existingReport in existingReports)
        //    {
        //        if (!incomingReports.Any(r => r.ReportId == existingReport.ReportId && r.ReportId != 0))
        //        {
        //            _dataContext.reportsList.Remove(existingReport);
        //        }
        //    }

        //    // הוספה או עדכון דיווחים קיימים
        //    foreach (var incomingReport in incomingReports)
        //    {
        //        var existingReport = existingReports.FirstOrDefault(r => r.ReportId == incomingReport.ReportId && r.ReportId != 0);

        //        if (existingReport != null)
        //        {
        //            _dataContext.Entry(existingReport).CurrentValues.SetValues(incomingReport);
        //        }
        //        else
        //        {
        //            incomingReport.EmpId = employee.Id;
        //            _dataContext.reportsList.Add(incomingReport);
        //        }
        //    }
        //    _logger.LogInformation($"Successfully updated employee with ID {employee.Id} in the database.");
        //    await _dataContext.SaveChangesAsync();
        //}///לפני השינוי למחלקה המקורית בלי הדיטיאוו
        public async Task<Employee> PostAsync(Employee employee)
        {
            _logger.LogInformation($"Starting to create new employee");

            if (employee == null)
            {
                throw new Exception("Employee is null");
            }

            if (string.IsNullOrEmpty(employee.UserName) || string.IsNullOrEmpty(employee.Password))
            {
                throw new Exception("Username and password are required for new employees.");
            }

            // הצפנת הסיסמה
            employee.Password = _passwordHasher.HashPassword(employee, employee.Password);
           // employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);

            _dataContext.empList.Add(employee);
            await _dataContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully created employee with ID {employee.Id} in the database.");
            return employee;
        }

        public async Task PutAsync(int id, Employee employee)
        {
            _logger.LogInformation($"Starting to update employee with ID {id}");

            if (employee == null)
            {
                throw new Exception("Employee is null");
            }

            var existingEmployee = await _dataContext.empList.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with id {id} not found");
            }

            // עדכון השדות
            // _dataContext.Entry(existingEmployee).CurrentValues.SetValues(employee);
            existingEmployee.UserName = employee.UserName;
            existingEmployee.Status = employee.Status;

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Age = employee.Age;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Seniority = employee.Seniority;
            

            // טיפול בסיסמה
            if (!string.IsNullOrEmpty(employee.Password) && employee.Password != existingEmployee.Password)
            {
                //existingEmployee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
                // הצפנת הסיסמה החדשה באמצעות PasswordHasher
                existingEmployee.Password = _passwordHasher.HashPassword(existingEmployee, employee.Password);
            }
        
            var incomingReports = employee.reportLst ?? new List<Report>();
            var existingReports = existingEmployee.reportLst.ToList();

            // מחיקת דיווחים שהוסרו
            foreach (var existingReport in existingReports)
            {
                if (!incomingReports.Any(r => r.ReportId == existingReport.ReportId && r.ReportId != 0))
                {
                    _dataContext.reportsList.Remove(existingReport);
                }
            }

            // הוספה או עדכון דיווחים קיימים
            foreach (var incomingReport in incomingReports)
            {
                var existingReport = existingReports.FirstOrDefault(r => r.ReportId == incomingReport.ReportId && r.ReportId != 0);

                if (existingReport != null)
                {
                    _dataContext.Entry(existingReport).CurrentValues.SetValues(incomingReport);
                }
                else
                {
                    incomingReport.EmpId = id; // קישור הדיווח לעובד הנוכחי
                    _dataContext.reportsList.Add(incomingReport);
                }
            }
            _dataContext.Entry(existingEmployee).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated employee with ID {id} in the database.");
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

        /// <summary>
        /// JWT
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //public async Task<Employee> AuthenticateAsync(string userName, string password)
        //{
        //    var employee = await _dataContext.empList.FirstOrDefaultAsync(e => e.UserName == userName);

        //    if (employee == null)
        //    {
        //        return null;
        //    }

        //    // אימות הסיסמה באמצעות PasswordHasher
        //    var verificationResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, password);

        //    if (verificationResult == PasswordVerificationResult.Success)
        //    {
        //        return employee;
        //    }

        //    return null;
        //}
        //public async Task<Employee> AuthenticateAsync(string userName, string password)
        //{
        //    var employee = await _dataContext.empList.FirstOrDefaultAsync(e => e.UserName == userName);

        //    if (employee == null)
        //    {
        //        return null;
        //    }

        //    if (BCrypt.Net.BCrypt.Verify(password, employee.Password))
        //    {
        //        return employee;
        //    }

        //    return null;
        //}

        //public async Task<Employee> AuthenticateAsync(string userName, string password)
        //{
        //    _logger.LogInformation("Attempting to authenticate user: {UserName}", userName);

        //    try
        //    {
        //        var employee = await _dataContext.empList.FirstOrDefaultAsync(e => e.UserName == userName);

        //        if (employee == null)
        //        {
        //            _logger.LogWarning("Authentication failed: User '{UserName}' not found.", userName);
        //            return null;
        //        }

        //        var verificationResult = _passwordHasher.VerifyHashedPassword(employee, employee.Password, password);

        //        if (verificationResult == PasswordVerificationResult.Success)
        //        {
        //            _logger.LogInformation("Authentication successful for user: {UserName}", userName);
        //            return employee;
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Authentication failed: Invalid password for user '{UserName}'.", userName);
        //            return null;
        //        }
        //    }
        //    catch (global::System.Exception ex)
        //    {
        //        _logger.LogError(ex, "Error during authentication for user: {UserName}", userName);
        //        throw;
        //    }
        //}
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
