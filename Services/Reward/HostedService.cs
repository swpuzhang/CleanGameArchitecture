using MassTransit;
using Microsoft.Extensions.Hosting;
using Reward.Domain.Manager;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reward
{
    public class HostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly IGameActivityConfigRepository _game;
        private readonly IRegisterRewardConfigRepository _register;
        private readonly ILoginRewardConfigRepository _login;
        private readonly IBankruptcyConfigRepository _bankrupt;
        private readonly IInviteRewardConfigRepository _invite;

        public HostedService(IBusControl busControl, 
            IGameActivityConfigRepository game, 
            IRegisterRewardConfigRepository register, 
            ILoginRewardConfigRepository login, 
            IBankruptcyConfigRepository bankrupt, 
            IInviteRewardConfigRepository invite)
        {
            _busControl = busControl;
            _game = game;
            _register = register;
            _login = login;
            _bankrupt = bankrupt;
            _invite = invite;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RewardManager.LoadConfig(_game, _register, _login, _bankrupt, _invite);
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
