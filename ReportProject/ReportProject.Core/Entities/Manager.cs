using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class Manager: Employee
    {
        private Login login { get; set; }
        private List<Employee> employees { get; set; }

    }
}
