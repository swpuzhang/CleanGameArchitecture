using Account.Domain.Manager;
using Account.Infrastruct.Repository;
using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Account
{
    public class HostedService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILevelConfigRepository _repository;

        public HostedService(IBusControl busControl, ILevelConfigRepository repository)
        {
            _busControl = busControl;
            _repository = repository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            LevelManager.LoadConfig(_repository);
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
