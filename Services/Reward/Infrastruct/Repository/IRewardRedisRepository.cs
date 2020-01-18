
using Commons.Db.Redis;
using Commons.DiIoc;
using Reward.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reward.Infrastruct.Repository
{
    
    public interface IRewardRedisRepository : IRedisRepository, IDependency
    {
        Task SetUserRegiserReward(RegisterRewardInfo info);
        Task<RegisterRewardInfo> GetUserRegiserReward(long id);
        Task<LoginRewardInfo> GetLoginReward(DateTime date, long id);

        Task SetUserLoginReward(DateTime date, LoginRewardInfo info);
        Task<BankruptcyInfo> GetBankruptcyInfo(DateTime date, long id);
        Task SetBankruptcyInfo(DateTime date, BankruptcyInfo info);

        Task SetInviteFriend(long id, string platform);
        Task RemovInviteFriend(IEnumerable<long> allInviter, string platform);
        Task<IEnumerable<long>> GetInviter(string platform);
        Task<OneGameActivityInfo> GetGameActivity(DateTime time, long id, string activityId);
        Task<GameSubActInfo> GetGameActProgress(DateTime time, long id, string activityId, string subId);
        Task SetGameActProgress(DateTime time, long id, string activityId, string subId, GameSubActInfo subAct);
    }
}
