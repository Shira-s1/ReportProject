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
        //public Task<List<ReportDTO>> GetAsync();ניסוי
        public Task<List<Report>> GetAsync();//AAA- בשביל פעולת גט שתחזיר גם את האיידי של העובד
        // public Task<ReportDTO> GetAsync(int id);הורדה 


        Task<Report> GetReportByIdAsync(int id);//ניסוי גט
        // public Task<Report> PostAsync(Report report);
        public Task<Report> PostAsync(int empId, Report report);//tryyyy
        public Task PutAsync(int id, ReportDTO reportDto);
        public Task DeleteAsync(int id);
    }
}
