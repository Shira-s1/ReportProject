using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class Report
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; } 

        //[ForeignKey("Employee")] // מצביע על נכס הניווט
        //public int EmpId { get; set; }//id of emp (מפתח זר לטבלת Employee)
        public TimeSpan ClockInTime { get; set; } // שעת כניסה
        public TimeSpan ClockOutTime { get; set; } // שעת יציאה                                                  
        public Absence TypeOfVacation { get; set; }//Type Of Vacation-work/sick/vacation                                                 
        public DateOnly startDate { get; set; }//תאריך התחלה
        public DateOnly EndtDate { get; set; }//תאריך גמר 
        public Employee Employee { get; set; }//כדי ליצור קשר בין העובד לדיווח
    }
}
