using Commons.Extenssions;
using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Commons.Db.Redis;
using Commons.Tools.KeyGen;
using Commons.Tools.Time;

namespace Reward.Infrastruct
{
    public class RewardRedisRepository : RedisRepository, IRewardRedisRepository
    {

        public Task<RegisterRewardInfo> GetUserRegiserReward(long id)
        {
            return RedisOpt.GetStringAsync<RegisterRewardInfo>(KeyGenTool.GenUserKey(id, nameof(RegisterRewardInfo)));
        }

        public Task SetUserRegiserReward(RegisterRewardInfo info )
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserKey(info.Id, nameof(RegisterRewardInfo)), info, TimeSpan.FromDays(5));
        }

        public Task<LoginRewardInfo> GetLoginReward(DateTime date, long id)
        {
            return RedisOpt.GetStringAsync<LoginRewardInfo>
                (KeyGenTool.GenUserWeekKey(date, id, nameof(LoginRewardInfo)));
        }

        public Task SetUserLoginReward(DateTime date, LoginRewardInfo info)
        {
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserWeekKey(date, info.Id, nameof(LoginRewardInfo)), info, TimeSpan.FromDays(8));
        }

        public Task<BankruptcyInfo> GetBankruptcyInfo(DateTime date, long id)
        {
            return RedisOpt.GetStringAsync<BankruptcyInfo>
                (KeyGenTool.GenUserDayKey(date, id, nameof(BankruptcyInfo)));
        }

        public Task SetBankruptcyInfo(DateTime date, BankruptcyInfo info)
        {
            
            return RedisOpt.SetStringAsync(KeyGenTool.GenUserDayKey
                (date, info.Id, nameof(BankruptcyInfo)), info, TimeSpan.FromDays(1));
        }

        public  async Task SetInviteFriend(long id, string platform)
        {
            string keyInvited = KeyGenTool.GenKey(platform, "Invited");
            string keyInviter = KeyGenTool.GenUserKey(id, "Inviter");
            var t1 = RedisOpt.AddZsetValueAsync(keyInvited, id.ToString(), 
                 DateTime.Now.ToTimeStamp());
            var t2 = RedisOpt.AddZsetValueAsync(keyInviter, platform,
                DateTime.Now.ToTimeStamp());
            await Task.WhenAll(t1, t2);
            var t3 = RedisOpt.ExpiryAsync(keyInvited, TimeSpan.FromDays(30));
            var t4 = RedisOpt.ExpiryAsync(keyInviter, TimeSpan.FromDays(30));
            var t5 = RedisOpt.DeleteZsetValueRangeAsync(keyInvited, 0, DateTime.Now.ToTimeStamp() - TimeSpan.FromDays(30).TotalSeconds); 
            var t6 = RedisOpt.DeleteZsetValueRangeAsync(keyInviter, 0, DateTime.Now.ToTimeStamp() - TimeSpan.FromDays(30).TotalSeconds);
            await Task.WhenAll(t3, t4, t5, t6);
        }

        public async Task<IEnumerable<long>> GetInviter(string platform)
        {

            var allInviter = await RedisOpt.GetZsetAllKeyAsync(KeyGenTool.GenKey
                 (platform, "Invited"));
            return allInviter.Select(x => long.Parse(x));
          
        }

        public Task RemovInviteFriend(IEnumerable<long> allInviter, string platform)
        {
            List<Task> allTasks = new List<Task>
            {
                RedisOpt.DeleteKeyAsync(KeyGenTool.GenKey
                 (platform, "Invited"))
            };
            foreach (var oneInviter in allInviter)
            {
                allTasks.Add(RedisOpt.DeleteZsetValueAsync(KeyGenTool.GenUserKey
                (oneInviter, "Inviter"), platform));
            }
            return Task.WhenAll(allTasks);
        }

        public async Task<OneGameActivityInfo> GetGameActivity(DateTime time, long id, string activityId)
        {
            var dic = await RedisOpt.GetHashAllAsync<string, GameSubActInfo>
                (KeyGenTool.GenUserDayKey(time, id, "GameActivity", activityId));
            return new OneGameActivityInfo(activityId, dic.ToDictionary(x =>x.Key, y  => y.Value));
        }

        public async Task<GameSubActInfo> GetGameActProgress(DateTime time, long id, string activityId, string subId)
        {
            GameSubActInfo subAct = await RedisOpt.GetHashValueAsync<GameSubActInfo>
                (KeyGenTool.GenUserDayKey(time, id, "GameActivity", activityId), subId);
            return subAct;
        }

        public Task SetGameActProgress(DateTime time, long id, string activityId,
            string subId, GameSubActInfo subAct)
        {

            return RedisOpt.AddHashValueAsync(KeyGenTool.GenUserDayKey(time, id,
                "GameActivity", activityId), subId, subAct, TimeSpan.FromDays(1));
        }
    }
}
