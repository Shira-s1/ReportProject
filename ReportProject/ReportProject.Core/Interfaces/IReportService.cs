using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Interfaces
{
    public interface IReportService
    {
     
        public Task<List<Report>> GetAsync();//AAA- בשביל פעולת גט שתחזיר גם את האיידי של העובד
       
        Task<Report> GetReportByIdAsync(int id);
       
        public Task<Report> PostAsync(int empId, Report report);
        public Task PutAsync(int id, ReportDTO reportDto);
        public Task DeleteAsync(int id);
    }
}
