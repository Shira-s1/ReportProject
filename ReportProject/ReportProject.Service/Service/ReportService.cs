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
        public async Task<List<ReportDTO>> GetAsync()
        {
            _logger.LogInformation("Attempting to retrieve all reports.");

            var reports = await _dataContext.reportsList
                .ToListAsync();

            _logger.LogInformation($"Successfully retrieved {reports.Count} reports.");
            return _mapper.Map<List<ReportDTO>>(reports);
        }

        //public async Task<ReportDTO> GetAsync(int id)
        //{
        //    var report = await _dataContext.reportsList
        //        .FirstOrDefaultAsync(r => r.ReportId == id);
        //    return _mapper.Map<ReportDTO>(report);
        //}
        public async Task<ReportDTO> GetAsync(int id)
        {
            _logger.LogInformation($"Attempting to retrieve report with ID: {id}");

            var reportEntity = await _dataContext.reportsList
                .FirstOrDefaultAsync(r => r.ReportId == id);

            if (reportEntity == null)
            {
                _logger.LogWarning($"Report with ID: {id} not found.");
                return null;
            }

            _logger.LogInformation($"Successfully retrieved report with ID: {id}.");
            return _mapper.Map<ReportDTO>(reportEntity);
        }


        public async Task<Report> PostAsync(Report report)//הוספה
        {
            if (report == null)
            {
                _logger.LogInformation("The object is null, Enter items");
                throw new Exception("Report is null");
            }
            if (report.EndtDate < report.startDate)
            {
                _logger.LogInformation("The report end date cannot be before the start date.");
                throw new ArgumentException("Fix your dates.");
            }
            //if(report.ClockInTime < report.ClockOutTime)
            //{
            //    _logger.LogInformation("Check-in time is after check-out time.");
            //   // throw new ArgumentException("The end date is less than the start date.");
            //}
            _dataContext.reportsList.Add(report);
            await _dataContext.SaveChangesAsync();
            return report; // Return the created entity
        }

        public async Task PutAsync(int id, ReportDTO reportDto)
        {
            var existingReport = await _dataContext.reportsList.FindAsync(id);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Report with id {id} not found");
            }

            existingReport.startDate = reportDto.startDate;
            existingReport.EndtDate = reportDto.EndtDate;
            existingReport.ClockInTime = reportDto.ClockInTime;
            existingReport.ClockOutTime = reportDto.ClockOutTime;
            existingReport.TypeOfVacation = reportDto.TypeOfVacation;
            existingReport.EmpId = reportDto.EmpId; // ודא שאתה רוצה לאפשר עדכון EmpId

            if (existingReport.EndtDate < existingReport.startDate)
            {
                throw new ArgumentException("The report end date cannot be before the start date.");
            }
            if (existingReport.ClockInTime > existingReport.ClockOutTime) // תיקון התנאי
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
