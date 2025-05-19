using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReportProject.Core.DTOs;
using ReportProject.Core.Entities;
using ReportProject.Core.Interfaces;
using ReportProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Service.Service
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ReportService(IMapper mapper, ILogger<ReportService> logger, DataContext dataContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<Report>> GetAsync()
        {
            _logger.LogInformation("Attempting to retrieve all reports.");

            var reports = await _dataContext.reportsList
                .Include(r => r.Employee) // <-- הוספת השורה הזו
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved {reports.Count} reports.");
            return reports;
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            _logger.LogInformation($"Attempting to retrieve report entity with ID: {id}");
            var reportEntity = await _dataContext.reportsList.Include(r => r.Employee).FirstOrDefaultAsync(r => r.ReportId == id);
            if (reportEntity == null)
            {
                _logger.LogWarning($"Report entity with ID: {id} not found.");
                return null;
            }
            _logger.LogInformation($"Successfully retrieved report entity with ID: {id}.");
            return reportEntity;
        }

        public async Task<Report> PostAsync(int empId, Report report)
        {
            if (report == null)
            {
                _logger.LogInformation("The object is null, Enter items");
                throw new ArgumentNullException(nameof(report), "Report object cannot be null.");
            }
            if (report.EndtDate < report.startDate)
            {
                _logger.LogInformation("The report end date cannot be before the start date.");
                throw new ArgumentException("The report end date cannot be before the start date.");
            }

            // אחזר את העובד המתאים מבסיס הנתונים
            var employee = await _dataContext.empList.FindAsync(empId);
            if (employee == null)
            {
                _logger.LogError($"Employee with ID {empId} not found.");
                throw new ArgumentException($"Employee with ID {empId} not found.");
            }

            // קשר את העובד לדיווח
            report.Employee = employee;

            // הוסף את הדיווח לרשימת הדיווחים של העובד (אופציונלי, תלוי בהגדרות הקשר ב-EF Core)
            // אם הקשר מוגדר כראוי, EF Core עשוי לטפל בזה אוטומטית
            if (employee.reportLst == null)
            {
                employee.reportLst = new List<Report>();
            }
            employee.reportLst.Add(report);

            _dataContext.reportsList.Add(report);
            await _dataContext.SaveChangesAsync();
            return report;
        }


        public async Task PutAsync(int id, ReportDTO reportDto)//get the report by id and update it
        {
            var existingReport = await _dataContext.reportsList.FindAsync(id);//check add .Include() for employee;
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Report with id {id} not found");
            }

            existingReport.startDate = reportDto.startDate;
            existingReport.EndtDate = reportDto.EndtDate;
            existingReport.ClockInTime = reportDto.ClockInTime;
            existingReport.ClockOutTime = reportDto.ClockOutTime;
            existingReport.TypeOfVacation = reportDto.TypeOfVacation;
            // existingReport.EmpId = reportDto.EmpId;

            if (existingReport.EndtDate < existingReport.startDate)
            {
                throw new ArgumentException("The report end date cannot be before the start date.");
            }
            if (existingReport.ClockInTime > existingReport.ClockOutTime) 
            {
                _logger.LogInformation("Check-in time is after check-out time.");
                // throw new ArgumentException("Check-in time cannot be after check-out time.");
            }
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting to delete report with ID {id}.");

            var report = await _dataContext.reportsList.FindAsync(id);
            if (report != null)
            {
                _dataContext.reportsList.Remove(report);
                await _dataContext.SaveChangesAsync();
            }
            else
                _logger.LogInformation("The object is null, Enter items");
        }
    }
}
