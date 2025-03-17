using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Interfaces
{
    public interface IEnryAndExitService
    {
      
        public Task<List<EntryAndExit>> GetAsync();
        public Task<List<EntryAndExit>> GetAsync(int id);
        public Task PostAsync(EntryAndExit entryAndExit);
        public Task PutAsync(int id, EntryAndExit e);
        public  Task DeleteAsync(int id);
    }
}
