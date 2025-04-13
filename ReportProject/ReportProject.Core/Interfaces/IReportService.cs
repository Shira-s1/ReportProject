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
        public Task<List<ReportDTO>> GetAsync();
        public Task<ReportDTO> GetAsync(int id);
        public Task<Report> PostAsync(Report report);
        public Task PutAsync(int id, ReportDTO reportDto);
        public Task DeleteAsync(int id);
    }
}
