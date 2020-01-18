using Account.Domain.Entitys;
using Account.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Methods
{
    public static class AccountRepositoryHelper
    {
        public static async Task<AccountInfo> GetAccountInfo(long id, IAccountInfoRepository _accountRepository, IAllRedisRepository _redis)
        {
            var accountInfo = await _redis.GetAccountInfo(id);
            if (accountInfo == null)
            {
                accountInfo = await _accountRepository.GetByIdAsync(id);
                if (accountInfo != null)
                {
                    await _redis.SetAccountInfo(accountInfo);
                }
            }
            return accountInfo;
        }

        public static async Task<long?> GetIdByPlatForm(string platformId, IAccountInfoRepository _accountRepository, IAllRedisRepository _redis)
        {
            var checkInfo = await _redis.GetLoginCheckInfo(platformId);
            if (checkInfo == null)
            {
                AccountInfo accountInfo = await _accountRepository.GetByPlatform(platformId);
                if (accountInfo != null)
                {
                    await _redis.SetLoginCheckInfo(platformId, accountInfo);
                    return accountInfo.Id;
                }
                return null;
            }
            return checkInfo.Id;
        }
    }
}
