using Account.Infrastruct.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;
using CommonMessages.MqCmds;
using Account.Domain.Entitys;

namespace Account.Domain.ProcessEvents
{
    public class AccountEventHandler : INotificationHandler<FinishRegisterRewardEvent>
    {
        private readonly IAllRedisRepository _redisRep;
        private readonly IAccountInfoRepository _accountRep;

        public AccountEventHandler(IAllRedisRepository redisRep, IAccountInfoRepository accountRep)
        {
            _redisRep = redisRep;
            _accountRep = accountRep;
        }

        public async Task Handle(FinishRegisterRewardEvent notification, CancellationToken cancellationToken)
        {
            using var loker = _redisRep.Locker(KeyGenTool.GenUserKey(notification.Id, nameof(AccountInfo)));
            loker.Lock();
            AccountInfo info = await _redisRep.GetAccountInfo(notification.Id);
            if (info == null)
            {
                info = await _accountRep.GetByIdAsync(notification.Id);
            }
            info.FinishRegister();
            await Task.WhenAll(_accountRep.UpdateAsync(info), _redisRep.SetAccountInfo(info));

        }
    }
}
