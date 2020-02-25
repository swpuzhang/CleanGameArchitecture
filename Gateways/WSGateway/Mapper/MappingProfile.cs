using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSGateway.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServerRequest, ToAppRequest>().ReverseMap();
            CreateMap<GameServerRequest, ToAppRoomRequest>().ReverseMap();

        }
    }
}
