using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class Vacations
    { 
        //DateOnly- רק תאריך בלי שעות
        public int Id { get; set; }//id of emp
        public Absence TypeOfVacation { get; set; }//Type Of Vacation
       // public DateTime? ClockInDate { get; set; }// the date when you take vacation
        public DateOnly startDate { get; set; }//תאריך לקיחת החופשה 
        public DateOnly EndtDate { get; set; }//תאריך גמר החופשה
    
        public int sumOdDays { get; set; }// מספר ימי החופשה
    }
}
