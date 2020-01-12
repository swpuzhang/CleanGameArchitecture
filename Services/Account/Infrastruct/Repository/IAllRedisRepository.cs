using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Entitys;
using Commons.DiIoc;

namespace Account.Infrastruct.Repository
{
    public interface IAllRedisRepository : IRedisRepository, IDependency
    {
        Task<LoginCheckInfo> GetLoginCheckInfo(string platformAccount);

        Task SetLoginCheckInfo(string account, AccountInfo info);

        Task SetAccountInfo(AccountInfo info);

        Task<AccountInfo> GetAccountInfo(long id);

        Task<LevelInfo> GetLevelInfo(long id);

        Task<GameInfo> GetGameInfo(long id);

        Task SetLevelInfo(LevelInfo info);

        Task SetGameInfo(GameInfo info);
    }
}
