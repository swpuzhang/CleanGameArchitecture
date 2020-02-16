﻿using RoomMatch.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.Infrastruct.Repository
{
    public interface IRoomMatchInfoRepository : IMongoUserRepository<RoomMatchInfo>, IDependency
    {
       
    }
}
