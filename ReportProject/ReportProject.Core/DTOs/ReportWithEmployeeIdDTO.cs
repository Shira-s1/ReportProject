using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.DTOs
{
    public class ReportWithEmployeeIdDTO//להצגת הדיווח
    {
        public int ReportId { get; set; }
        public TimeSpan ClockInTime { get; set; }
        public TimeSpan ClockOutTime { get; set; }
        public Absence TypeOfVacation { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndtDate { get; set; }
        public int EmployeeId { get; set; }
    }
}
