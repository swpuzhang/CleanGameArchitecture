﻿using Account.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Infrastruct.Repository
{
    public interface IUserIdGenRepository : IMongoUserRepository<UserIdGenInfo>, IDependency
    {
        Task<Int64> GenNewId();
    }
}
