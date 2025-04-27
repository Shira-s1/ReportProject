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
            //        CreateMap<EmployeeDTO, Employee>();
            //        CreateMap<Employee, EmployeeDTO>()
            //            .ForMember(dest => dest.Password, opt => opt.Ignore());
            //        CreateMap<Report, ReportDTO>().ReverseMap();


            //        CreateMap<Employee, EmployeeGetDTO>();


            //        CreateMap<Report, ReportWithEmployeeIdDTO>()
            //.ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id));



            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.reportLst, opt => opt.MapFrom(src => src.reportLst));
            CreateMap<Employee, EmployeeGetDTO>()
                .ForMember(dest => dest.reportLst, opt => opt.MapFrom(src => src.reportLst));
            CreateMap<CreateEmployeeDTO, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // עבור יצירת עובד בלבד
                                                                  // CreateMap<CreateEmployeeWithReportsDTO, Employee>() // ייתכן ולא תשתמש בו כעת
                                                                  //     .ForMember(dest => dest.Id, opt => opt.Ignore())
                                                                  //     .ForMember(dest => dest.reportLst, opt => opt.Ignore());

            // Report Mappings
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Report, ReportWithEmployeeIdDTO>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id));
            CreateMap<ReportDTO, Report>()
                .ForMember(dest => dest.ReportId, opt => opt.Ignore()); // התעלם מה-ReportId בעת מיפוי מבקשה
        }
    }
}

