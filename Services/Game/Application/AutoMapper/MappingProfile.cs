using Game.Domain.Entitys;
using Game.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameInfoVm, GameInfo>().ReverseMap();
        }
    }
}
