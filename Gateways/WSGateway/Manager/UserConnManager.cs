using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSGateway.Manager
{
    public static class UserConnManager
    {
        private static readonly Dictionary<long, string> _userConnIds = new Dictionary<long, string>();
        private static readonly Dictionary<string, long> _connUserIds = new Dictionary<string, long>();
        public static object _sync = new object();

        public static int GetUserCount()
        {
            lock (_sync)
            {
                return _userConnIds.Count;
            }
        }

        public static long GetUidByConn(string conn)
        {
            lock (_sync)
            {
                if (!_connUserIds.TryGetValue(conn, out var id))
                {
                    return 0;
                }
                return id;
            }
        }

        public static string GetConnByUid(long uid)
        {
            string conn = null;
            lock (_sync)
            {
                if (!_userConnIds.TryGetValue(uid, out conn))
                {
                    Log.Logger.Error($"some thing wrong GetConnByUid user:{uid} can't find conn");
                    return null;
                }
            }

            Log.Logger.Information($"GetConnByUid user:{uid} success conn:{conn}");
            return conn;
        }

        public static bool OnLogined(long userid, string conn)
        {
            lock (_sync)
            {
                if (!_userConnIds.TryAdd(userid, conn))
                {
                    Log.Logger.Error($"some thing wrong add user:{userid} conn:{conn} user has already logined");
                    return false;
                }
                if (!_connUserIds.TryAdd(conn, userid))
                {
                    Log.Logger.Error($"some thing wrong add user:{userid} conn:{conn} user has already logined");
                    return false;
                }
            }
            Log.Logger.Information($"add user:{userid} in connections success conn:{conn}");
            return true;
        }

        public static void OnDisconnected(string conn)
        {
            lock (_sync)
            {
                if (!_connUserIds.TryGetValue(conn, out var uid))
                {
                    Log.Logger.Error($"some thing wrong get user  conn:{conn}  error");
                    return;
                }
                _connUserIds.Remove(conn);
                if (!_userConnIds.Remove(uid))
                {
                    Log.Logger.Error($"some thing wrong remove user:{uid} conn:{conn} error");
                    return;
                }
                Log.Logger.Information($"remove user:{uid} from connections conn:{conn}  success");
            }

        }
    }
}
