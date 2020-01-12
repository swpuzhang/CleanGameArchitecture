using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Entitys
{
    public class UserIdGenInfo : UserEntity
    {
        public long UserId { get; private set; }
    }
}
