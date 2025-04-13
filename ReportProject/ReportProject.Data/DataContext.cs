using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ReportProject.Data
{
    public class DataContext: DbContext
    {
        public DbSet<Employee> empList { get; set; }
        public DbSet<Report> reportsList { get; set; }
        
        private readonly IConfiguration _configuration;
        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            //  optionBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Data_db")
            // optionBuilder.UseSqlServer(@"Server=(localdb)\ProjectModels;Database=Data_db")
            //LogTo(Console.WriteLine, LogLevel.Information);
            // if (!optionBuilder.IsConfigured)
            // optionBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
            optionBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));//Configuration
        }
    }
}
