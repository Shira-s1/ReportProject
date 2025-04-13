using AutoMapper;
using ReportProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportProject.Core.DTOs
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EmployeeDTO, Employee>(); // מיפוי מ-DTO ל-Entity
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<Report, ReportDTO>().ReverseMap();

            // מיפוי חדש עבור תגובות GET של עובדים עם שם קצר יותר
            CreateMap<Employee, EmployeeGetDTO>();
        }
    }
}
