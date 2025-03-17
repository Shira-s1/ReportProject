using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class EntryAndExit
    {
        //DateTime מייצג שעה ותאריך 
        public int Id { get; set; }//id of emp
        public DateOnly ClockInDate { get; set; } // תאריך 
        public TimeSpan ClockInTime { get; set; } // שעת כניסה

        //public DateTime? ClockOutDate { get; set; } // תאריך יציאה
        public TimeSpan ClockOutTime { get; set; } // שעת יציאה להפסקה


    }
}
