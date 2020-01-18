using Account.Infrastruct.Repository;
using AutoMapper;
using CommonMessages.MqEvents;
using MassTransit;
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
        private readonly IBusControl _mqBus;
        private readonly IMapper _mapper;
        public LoginEventHandler(IAllRedisRepository accountRedisRep, IAccountInfoRepository accountRep, IBusControl mqBus, IMapper mapper)
        {
            _redisRep = accountRedisRep;
            _accountRep = accountRep;
            _mqBus = mqBus;
            _mapper = mapper;
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
                    _ = _mqBus.Publish(_mapper.Map<RegistMqEvent>(notification.AccounResponse));
                }
                else
                {
                    //通知登录
                    _ = _mqBus.Publish(_mapper.Map<LoginMqEvent>(notification.AccounResponse));
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
