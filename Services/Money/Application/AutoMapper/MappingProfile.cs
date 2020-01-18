using Money.Domain.Entitys;
using Money.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonMessages.MqCmds;

namespace Money.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MoneyInfo, MoneyMqResponse>().ReverseMap();
        }
    }
}
