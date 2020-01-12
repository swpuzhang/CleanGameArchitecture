using Account.Infrastruct.Repository;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Domain.ProcessEvents
{
    public class LoginEventHandler : INotificationHandler<LoginEvent>
    {
        private readonly IAllRedisRepository _redisRep;
        private readonly IAccountInfoRepository _accountRep;
        public LoginEventHandler(IAllRedisRepository accountRedisRep, IAccountInfoRepository accountRep)
        {
            _redisRep = accountRedisRep;
            _accountRep = accountRep;
        }
        public Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            if (notification.IsNeedUpdate)
            {
                _accountRep.Update(notification.Info);
            }
            try
            {
                if (notification.IsRegister)
                {
                    //通知注册
                   
                }
                else
                {
                    //通知登录
                   
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Publish LoginMqEvent exception {ex.Message}");
            }
            Log.Information("Handle");
            _redisRep.SetAccountInfo(notification.Info);
            _redisRep.SetLoginCheckInfo(notification.Info.PlatformAccount, notification.Info);
            return Task.CompletedTask;
        }
    }
}
