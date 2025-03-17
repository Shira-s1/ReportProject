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
        public Task<List<EmployeeDTO>> GetAsync();
        public Task<EmployeeDTO> GetAsync(int id);
        public Task PostAsync(EmployeeDTO employeeDto);
        public Task PutAsync(int id, EmployeeDTO employeeDto);
        public Task DeleteAsync(int id);
    }
}
