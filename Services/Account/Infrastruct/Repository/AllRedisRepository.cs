using Account.Domain.Entitys;
using Commons.Db.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;

namespace Account.Infrastruct.Repository
{
    public class AllRedisRepository : RedisRepository, IAllRedisRepository
    {
        public Task<AccountInfo> GetAccountInfo(long id)
        {
            return RedisOpt.GetStringAsync<AccountInfo>(KeyGenTool.GenUserKey(id,
                nameof(AccountInfo)));
        }

        public Task<LoginCheckInfo> GetLoginCheckInfo(string platformAccount)
        {
            return RedisOpt.GetStringAsync<LoginCheckInfo>(KeyGenTool.GenKey(platformAccount,
                nameof(LoginCheckInfo)));
        }


        public Task SetAccountInfo(AccountInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id,
               nameof(AccountInfo)), info, TimeSpan.FromDays(7));
        }

        public Task SetLoginCheckInfo(string account, AccountInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenKey(account, nameof(LoginCheckInfo)),
               new LoginCheckInfo(info.Id, info.PlatformAccount, info.Type),
               TimeSpan.FromDays(7));
        }
        public Task<LevelInfo> GetLevelInfo(long id)
        {
            return RedisOpt.GetStringAsync<LevelInfo>(KeyGenTool
                .GenUserKey(id, LevelInfo.ClassName));
        }

        public Task SetLevelInfo(LevelInfo info)
        {
            RedisOpt.SetStringNoWait<LevelInfo>(KeyGenTool
                .GenUserKey(info.Id, LevelInfo.ClassName), info, TimeSpan.FromDays(7));
            return Task.CompletedTask;
        }

        public Task<GameInfo> GetGameInfo(long id)
        {
            return RedisOpt.GetStringAsync<GameInfo>(KeyGenTool.GenUserKey(id, GameInfo.ClassName));
        }

        public Task SetGameInfo(GameInfo info)
        {
            RedisOpt.SetStringNoWait(KeyGenTool
                .GenUserKey(info.Id, GameInfo.ClassName), info, TimeSpan.FromDays(7));
            return Task.CompletedTask;
        }
    }
}
