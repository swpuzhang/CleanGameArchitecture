using System;
using System.Collections.Generic;
using System.Text;

namespace CommonMessages.MqEvents
{
    public class HostInfoMqEvent
    {
        public HostInfoMqEvent(string host, int userCount)
        {
            Host = host;
            UserCount = userCount;
        }

        public string Host { get; private set; }
        public int UserCount { get; private set; }
    }
}
