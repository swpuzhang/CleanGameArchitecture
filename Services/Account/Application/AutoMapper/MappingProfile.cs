using Account.Domain.Entitys;
using Account.ViewModels;
using AutoMapper;
using Commons.MqEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountInfoVm, AccountInfo>().ReverseMap();
            CreateMap<AccountResponse, AccountResponseVm>()
                .ForMember(x => x.CurCoins, (map) => map.MapFrom(dto => dto.MoneyInfo.CurCoins))
                .ForMember(x => x.CurDiamonds, (map) => map.MapFrom(dto => dto.MoneyInfo.CurDiamonds))
                .ReverseMap();
            CreateMap<AccountDetailVm, OtherAccountDetailVm>().ReverseMap();
            CreateMap<GameInfoVm, GameInfo>().ReverseMap();
            CreateMap<LevelInfoVm, LevelInfo>().ReverseMap();
            CreateMap<MoneyInfoVm, MoneyInfo>().ReverseMap();
            CreateMap<AccountResponse, LoginMqEvent>().ReverseMap();
            CreateMap<AccountResponse, RegistMqEvent>().ReverseMap();
        }
    }
}
