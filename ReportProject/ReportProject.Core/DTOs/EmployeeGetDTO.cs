using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.DTOs
{
    public class EmployeeGetDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Role Status { get; set; }
        public List<ReportDTO> reportLst { get; set; }
    }
}
