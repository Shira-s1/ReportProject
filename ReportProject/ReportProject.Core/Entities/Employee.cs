﻿using Microsoft.EntityFrameworkCore;
using ReportProject.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }//primary key
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public int Seniority { get; set; }//ותק

        public double Salary { get; set; }

        //check if he is managet or employee
        public Role Status { get; set; }//enum- manager / Employee
        public string UserName { get; set; }
        public string Password { get; set; }

        [InverseProperty("Employee")]
        public List<Report> reportLst { get; set; }//רשימת החופשות ושעות העבודה של העובד

    }


}

