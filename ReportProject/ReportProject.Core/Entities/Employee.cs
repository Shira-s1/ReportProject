using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class Employee
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int age { get; set; }
        public int seniority { get; set; }//ותק

        public DateTime? ClockInTime { get; set; }  // זמן כניסה
        public DateTime? ClockOutTime { get; set; }  // זמן יציאה
        public double Salary { get; set; }

        //check if he is managet or employee
        public Role Status { get; set; }//enum

    }
}
