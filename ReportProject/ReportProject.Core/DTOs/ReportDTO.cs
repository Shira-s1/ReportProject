using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.DTOs
{
    public class ReportDTO
    {
        [Key]
        public int ReportId { get; set; }

        //[ForeignKey("Employee")] // אם אתה רוצה לחשוף את המפתח הזר ב-DTO
        public int EmpId { get; set; }

        public TimeSpan ClockInTime { get; set; }
        public TimeSpan ClockOutTime { get; set; }
        public Absence TypeOfVacation { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly EndtDate { get; set; }
    }
}
