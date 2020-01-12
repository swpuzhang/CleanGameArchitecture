using ServiceTemplate.Domain.Entitys;
using ServiceTemplate.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTemplate.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServiceTemplateInfoVm, ServiceTemplateInfo>().ReverseMap();
        }
    }
}
