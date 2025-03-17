using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Interfaces
{
    public interface IVacationsService
    {
        public Task<List<Vacations>> GetAsync();
        public Task<List<Vacations>> GetAsync(int id);
        public Task PostAsync(Vacations v);
        public Task PutAsync(int id, Vacations vacation);
        public Task DeleteAsync(int id);
       
    }
}
