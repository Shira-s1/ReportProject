using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class EntryAndExitService: IEnryAndExitService
    {
        private readonly DataContext _dataContext;
       
        public EntryAndExitService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<EntryAndExit>> GetAsync()
        {
            try
            {
                var entryAndExit = await _dataContext.enterAndExitList.AsNoTracking().ToListAsync();
                return entryAndExit;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entries and exits.", ex);
            }
        }
        //to show all entry and exit of one emp
        public async Task<List<EntryAndExit>> GetAsync(int id)
        {
            try
            {
                var entriesAndExits = await _dataContext.enterAndExitList
                     .Where(e => e.Id == id)
                     .ToListAsync();
                return entriesAndExits;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving entries and exits for employee ID {id}.", ex);
            }
        }
        public async Task PostAsync(EntryAndExit entryAndExit)//add
        {
            try
            {
                if (entryAndExit == null)
                {
                    throw new ArgumentNullException(nameof(entryAndExit), "Entry and exit object cannot be null.");
                }

               

                _dataContext.enterAndExitList.Add(entryAndExit);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entry and exit record.", ex);
            }
        }

        //update
        public async Task PutAsync(int id, EntryAndExit e)
        {
            try
            {
                var existingEntryAndExit = await _dataContext.enterAndExitList.FindAsync(id);

                if (existingEntryAndExit == null)
                {
                    throw new Exception("Entry and exit record not found.");
                }

                // Update the properties of the existing record with the values from 'e'
                existingEntryAndExit.Id = e.Id;
                existingEntryAndExit.ClockInTime = e.ClockInTime;
                existingEntryAndExit.ClockInDate = e.ClockInDate;
                existingEntryAndExit.ClockOutTime = e.ClockOutTime;
              

                // Save the changes to the database
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during the update process
                throw new Exception("Error updating entry and exit record.", ex);
            }
        }
     

        public async Task DeleteAsync(int id)//to delete all his entry and exit
        {
            var entriesAndExits = _dataContext.enterAndExitList.Where(e => e.Id == id);
            if (entriesAndExits != null)
            {
                // Remove all found records
                _dataContext.enterAndExitList.RemoveRange(entriesAndExits);
                await _dataContext.SaveChangesAsync();
            }
        }

    }
}
