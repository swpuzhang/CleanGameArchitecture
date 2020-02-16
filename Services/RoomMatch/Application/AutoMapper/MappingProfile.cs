using RoomMatch.Domain.Entitys;
using RoomMatch.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomMatchInfoVm, RoomMatchInfo>().ReverseMap();
        }
    }
}
