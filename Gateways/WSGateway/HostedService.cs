using CommonMessages.MqEvents;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WSGateway.Manager;

namespace WSGateway
{
    public class HostedService
    {
        private readonly IBusControl _busControl;
        private Timer _timer;
        private readonly IConfiguration _configration;


        public HostedService(IBusControl busControl, IConfiguration configration)
        {
            _busControl = busControl;
            _configration = configration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return _busControl.StopAsync(cancellationToken);
        }

        private void DoWork(object state)
        {
            _busControl.Publish(new HostInfoMqEvent(_configration["WSHost"], UserConnManager.GetUserCount()));
        }
    }
}
