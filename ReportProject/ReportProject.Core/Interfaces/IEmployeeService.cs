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
        public Task<EmployeeGetDTO> GetAsync(int id);
        //public Task<Employee> PostAsync(Employee employee);

        //public Task PutAsync(Employee employee);

        public Task<Employee> PostAsync(Employee employee);//BB
        public Task PutAsync(int id, Employee employee);//BB


        public Task DeleteAsync(int id);
       Task<Employee> AuthenticateAsync(string userName, string password);
    }
}
