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
       
       
        public Task<List<EmployeeGetDTO>> GetAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
       
        Task<Employee> PostEmployeeAsync(Employee employee);
       
        Task PutEmployeeAsync(Employee employee); 

        public Task DeleteAsync(int id);
       Task<Employee> AuthenticateAsync(string userName, string password);

        Task<Employee> GetEmployeeByUserNameAsync(string userName);//פונקציית עזר לבדיקה אם כבר קיים שם משתמש זהה

    }
}
