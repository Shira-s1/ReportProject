using ReportProject.Core.Entities;
using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }//primary key
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }

        //check if he is managet or employee
        public Role Status { get; set; }//enum-if he is manager or emp
    }
}
