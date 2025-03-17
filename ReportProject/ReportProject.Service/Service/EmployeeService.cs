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

namespace ReportProject.Service.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public EmployeeService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<List<EmployeeDTO>> GetAsync()
        {
            var employees = await _dataContext.empList.AsNoTracking().ToListAsync(); // קריאה אסינכרונית למסד הנתונים
            return _mapper.Map<List<EmployeeDTO>>(employees);
        }
        public async Task<EmployeeDTO> GetAsync(int id)
        {
            var emp = await _dataContext.empList.FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null)
            {
                return null;
            }

             return _mapper.Map<EmployeeDTO>(emp);
           
        }
        public async Task PostAsync(EmployeeDTO employeeDto)
        {
            if (employeeDto == null) 
                throw new Exception("Employee is null");

            var employee = _mapper.Map<Employee>(employeeDto);
            _dataContext.empList.Add(employee);
            await _dataContext.SaveChangesAsync();
        }
        public async Task PutAsync(int id, EmployeeDTO employeeDto)
        {
            var existingEmployee = await _dataContext.empList.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new Exception("Employee not found");
            }

            _mapper.Map(employeeDto, existingEmployee); // מיפוי לעובד קיים
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var employee = await _dataContext.empList.FindAsync(id);
            if (employee != null)
            {
                _dataContext.empList.Remove(employee);
                await _dataContext.SaveChangesAsync();
            }
        }
       
    }
}
