using Microsoft.AspNetCore.Mvc;
using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Interfaces
{
    public interface IEmployeeService
    {
        //public Task<List<EmployeeDTO>> GetAsync();
        //EmployeeDTO GetAsync(int id);
        //Task GetAsync();
        public Task<List<EmployeeGetDTO>> GetAsync();
        //public Task<EmployeeGetDTO> GetAsync(int id);ניסוי עם גט
        Task<Employee> GetEmployeeByIdAsync(int id);
        //public Task<Employee> PostAsync(Employee employee);

        //public Task PutAsync(Employee employee);

        //public Task<Employee> PostAsync(Employee employee);//BB
        //  Task<Employee> PostEmployeeWithReportsAsync(Employee employee); // - לא פעיל שיטה חדשה
        Task<Employee> PostEmployeeAsync(Employee employee);
        //public Task PutAsync(int id, Employee employee);//BB
        Task PutEmployeeAsync(Employee employee); // שם הפונקציה שונה

        public Task DeleteAsync(int id);
       Task<Employee> AuthenticateAsync(string userName, string password);

        Task<Employee> GetEmployeeByUserNameAsync(string userName);//פונקציית עזר לבדיקה אם כבר קיים שם משתמש זהה

    }
}
