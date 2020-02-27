using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Manager
{
    public static class WSHostManager
    {
        class HostInfo : IComparable<HostInfo>
        {
            public HostInfo(string host, int userCount, DateTime lastAlive)
            {
                Host = host;
                UserCount = userCount;
                LastAlive = lastAlive;
            }

            public string Host { get; set; }
            public int UserCount { get; set; }

            public DateTime LastAlive { get; set; }

            public int CompareTo(HostInfo other)
            {

                int ret = UserCount.CompareTo(other.UserCount);
                if (ret != 0)
                {
                    return ret;
                }
                ret = -LastAlive.CompareTo(other.LastAlive);
                if (ret != 0)
                {
                    return ret;
                }
                return Host.CompareTo(other.Host);


            }
        }



        class HostInfoSortByAlive : HostInfo, IComparable<HostInfoSortByAlive>
        {
            public HostInfoSortByAlive(string host, int userCount, DateTime lastAlive)
                : base(host, userCount, lastAlive)
            {

            }
            public int CompareTo(HostInfoSortByAlive other)
            {
                int ret = LastAlive.CompareTo(other.LastAlive);
                if (ret == 0)
                {
                    return Host.CompareTo(other.Host);
                }
                return ret;
            }
        }

        public static void CleanBadHost()
        {
            DateTime tnow = DateTime.Now;
            List<string> willRemoveHost = new List<string>();
            lock (_syncLock)
            {
                foreach (var one in _hostAliveInfos)
                {
                    if (tnow - one.LastAlive > TimeSpan.FromSeconds(10))
                    {
                        willRemoveHost.Add(one.Host);
                    }
                    else
                    {
                        break;
                    }
                }
                foreach (var one in willRemoveHost)
                {
                    _allHost.Remove(one, out var pairInfo);
                    _hostInfos.Remove(pairInfo.Key);
                    _hostAliveInfos.Remove(pairInfo.Value);
                }
            }

        }

        public static void OnNotifyHostInfo(string host, int userCount)
        {
            lock (_syncLock)
            {
                DateTime tnow = DateTime.Now;
                if (!_allHost.TryGetValue(host, out var hostPair))
                {
                    var newInfo = new HostInfo(host, userCount, tnow);
                    var newInfoAlive = new HostInfoSortByAlive(host, userCount, tnow);
                    hostPair = new KeyValuePair<HostInfo, HostInfoSortByAlive>(
                        newInfo, newInfoAlive);
                    _allHost.Add(host, hostPair);
                    _hostInfos.Add(newInfo);
                    _hostAliveInfos.Add(newInfoAlive);
                }

                else
                {
                    //hostPair.Key.LastAlive = tnow;
                    // 

                    _hostInfos.Remove(hostPair.Key);
                    hostPair.Key.UserCount = userCount;
                    hostPair.Key.LastAlive = tnow;
                    _hostInfos.Add(hostPair.Key);


                    _hostAliveInfos.Remove(hostPair.Value);
                    hostPair.Value.LastAlive = tnow;
                    hostPair.Value.UserCount = userCount;
                    _hostAliveInfos.Add(hostPair.Value);
                }
            }
        }

        public static string GetOneHost()
        {
            lock (_syncLock)
            {
                if (_hostInfos.Count == 0)
                {
                    return null;
                }
                return _hostInfos.First().Host;
            }
        }

        private readonly static SortedSet<HostInfo> _hostInfos = new SortedSet<HostInfo>();
        private readonly static SortedSet<HostInfoSortByAlive> _hostAliveInfos = new SortedSet<HostInfoSortByAlive>();
        private readonly static Dictionary<string, KeyValuePair<HostInfo, HostInfoSortByAlive>> _allHost =
            new Dictionary<string, KeyValuePair<HostInfo, HostInfoSortByAlive>>();
        private readonly static object _syncLock = new object();
    }

}
