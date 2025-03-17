using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReportProject.Core.Entities;
using ReportProject.Core.Enum;
using ReportProject.Core.Interfaces;
using ReportProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Service.Service
{
    public class VacationsService :IVacationsService
    {
        private readonly DataContext _dataContext;
      
        public VacationsService(DataContext dataContext)
        {
            _dataContext = dataContext;
           
        }
        public async Task<List<Vacations>> GetAsync()
        {
            try
            {
                var vacations = await _dataContext.vacationsList.AsNoTracking().ToListAsync();
                return vacations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in vacations.", ex);
            }
        }
        public async Task<List<Vacations>> GetAsync(int id)
        {
            try
            {
                var vacations = await _dataContext.vacationsList
                    .Where(v => v.Id == id)
                    .ToListAsync();
                return vacations;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vacations for employee ID {id}.", ex);
            }
        }
        public async Task PostAsync(Vacations v)//add
        {
            try
            {
                if (v == null)
                    throw new Exception("Vacation is null");
                //
                if (v.EndtDate < v.startDate)
                {
                    throw new ArgumentException("The vacation end date cannot be before the start date.");
                }

                _dataContext.vacationsList.Add(v);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entry and exit record.", ex);
            }
        }
        public async Task PutAsync(int id, Vacations vacation)
        {
            try
            {
                var existingVacation = await _dataContext.vacationsList.FindAsync(id);

                if (existingVacation == null)
                {
                    throw new KeyNotFoundException($"Vacation record with ID {id} not found.");
                }
                if (vacation.EndtDate < vacation.startDate)
                {
                    throw new ArgumentException("The vacation end date cannot be before the start date.");
                }
                if (vacation.EndtDate.DayNumber - vacation.startDate.DayNumber + 1 < 1)
                {
                    throw new ArgumentException("The vacation must be at least one day.");
                }
                existingVacation.Id = vacation.Id;
                existingVacation.TypeOfVacation = vacation.TypeOfVacation;
                existingVacation.startDate = vacation.startDate;
                existingVacation.EndtDate = vacation.EndtDate;
                existingVacation.sumOdDays = vacation.sumOdDays;

                await _dataContext.SaveChangesAsync();
            }
            catch (KeyNotFoundException ex)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating vacation record.", ex);
            }
        }
     

        public async Task DeleteAsync(int id)//to delete all his entry and exit
        {
            try
            {
                var vacationExits = _dataContext.enterAndExitList.Where(e => e.Id == id);
                if (vacationExits != null)
                {
                    // Remove all found records
                    _dataContext.enterAndExitList.RemoveRange(vacationExits);
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting vacation records.", ex);
            }
        }
    }
}
