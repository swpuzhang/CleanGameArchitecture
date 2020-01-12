using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace Commons.Db.Redis
{
    public static class RedisOpt
    {
        private static readonly Lazy<ConnectionMultiplexer> _redis = 
            new Lazy<ConnectionMultiplexer>(()=>ConnectionMultiplexer.Connect(_connString));
        private static string _connString;
        /// <summary>
        /// 第一次调用start延迟加载
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Start( string connectionString)
        {
            _connString = connectionString;
            _redis.Value.GetDatabase();
        }

        #region redis锁操作
        public static bool LockTake(string key, string token, TimeSpan expireMs)
        {
            return _redis.Value.GetDatabase().LockTake(key, token, expireMs, CommandFlags.PreferMaster);
        }

        public static Task<bool> LockTakeAsync(string key, string token, TimeSpan expireMs)
        {
            return _redis.Value.GetDatabase().LockTakeAsync(key, token, expireMs, CommandFlags.PreferMaster);
        }

        public static void LockRelease(string key, string token)
        {
            _redis.Value.GetDatabase().LockRelease(key, token, CommandFlags.PreferMaster);
        }

        public static async Task LockReleaseAsync(string key, string token)
        {
            await _redis.Value.GetDatabase().LockReleaseAsync(key, token, CommandFlags.PreferMaster);

        }
        #endregion

        #region key相关操作
        public static bool IsKeyExist(string key)
        {
            return _redis.Value.GetDatabase().KeyExists(key, CommandFlags.PreferMaster);
        }

        public static Task<bool> IsKeyExistAsync(string key)
        {
            return _redis.Value.GetDatabase().KeyExistsAsync(key, CommandFlags.PreferMaster);
        }

        public static Task<bool> IsKeyExistAsync(string key, TimeSpan expiry)
        {
            return _redis.Value.GetDatabase().KeyExpireAsync(key, expiry, CommandFlags.PreferMaster);
        }

        public static bool Expiry(string key, TimeSpan? expiry = null)
        {
            return _redis.Value.GetDatabase().KeyExpire(key, expiry, CommandFlags.PreferMaster);
        }

        public static Task ExpiryAsync(string key, TimeSpan? expiry = null)
        {
            return _redis.Value.GetDatabase().KeyExpireAsync(key, expiry, CommandFlags.PreferMaster);
        }

        public static bool ExpiryNoWait(string key, TimeSpan? expiry = null)
        {
            return _redis.Value.GetDatabase().KeyExpire(key, expiry, flags: CommandFlags.PreferMaster);
        }
        public static bool DeleteKey(string key)
        {

            return _redis.Value.GetDatabase().KeyDelete(key, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> DeleteKeyAsync(string key)
        {
            return _redis.Value.GetDatabase().KeyDeleteAsync(key, flags: CommandFlags.PreferMaster);
        }

        public static void DeleteKeyNoWait(string key)
        {
            _redis.Value.GetDatabase().KeyDelete(key, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }
        #endregion 


        #region string类型操作
        public static bool SetString(string key, string value, TimeSpan? expiry = null)
        {
            return _redis.Value.GetDatabase().StringSet(key, value, expiry, flags: CommandFlags.PreferMaster);
        }

        public static void SetStringNoWait(string key, string value, TimeSpan? expiry = null)
        {
            _redis.Value.GetDatabase().StringSet(key, value, expiry, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        public static Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            return _redis.Value.GetDatabase().StringSetAsync(key, value, expiry, flags:CommandFlags.PreferMaster);
        }

        public static bool SetString<T>(string key, T obj, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            return _redis.Value.GetDatabase().StringSet(key, json, expiry, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> SetStringAsync<T>(string key, T obj, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            return _redis.Value.GetDatabase().StringSetAsync(key, json, expiry, flags: CommandFlags.PreferMaster);
        }

        public static void SetStringNoWait<T>(string key, T obj, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(obj);
            _redis.Value.GetDatabase().StringSet(key, json, expiry, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        public static T GetString<T>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            var result = _redis.Value.GetDatabase().StringGet(key, flags:flags);
            if (string.IsNullOrEmpty(result))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> GetStringAsync<T>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            var result = await _redis.Value.GetDatabase().StringGetAsync(key, flags:flags);
            if (string.IsNullOrEmpty(result))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public static string GetString(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().StringGet(key, flags:flags);
        }

        public static async Task<string> GetStringAsync(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return await _redis.Value.GetDatabase().StringGetAsync(key, flags: flags);

        }
        #endregion

        #region 哈希类型操作
        public static long GetHashCount(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().HashLength(key, flags: flags);
        }
        public static Task<long> GetHashCountAsync(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().HashLengthAsync(key, flags: flags);
        }

        public static bool AddHashValue(string key, string hashkey, string value)
        {
            return _redis.Value.GetDatabase().HashSet(key, hashkey, value, flags: CommandFlags.PreferMaster);
        }


        public static bool AddHashValue<T>(String key, string hashkey, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            return _redis.Value.GetDatabase().HashSet(key, hashkey, json, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> AddHashValueAsync<T>(String key, string hashkey, T t, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(t);

            var ret = _redis.Value.GetDatabase().HashSetAsync(key, hashkey, json, flags: CommandFlags.PreferMaster);
            if (expiry != null)
            {
                ExpiryNoWait(key, expiry);
            }

            return ret;
        }

        public static void AddHashValueNoWait<T>(String key, string hashkey, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            _redis.Value.GetDatabase().HashSet(key, hashkey, json, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        public static void AddHashList<T>(string key,  IEnumerable<KeyValuePair<string, T>> values)
        {
            var entrys = values.Select(p => new HashEntry(p.Key, JsonConvert.SerializeObject(p.Value)));
            _redis.Value.GetDatabase().HashSet(key, entrys.ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static async Task AddHashListAsync<Tkey, T>(string key, IEnumerable<KeyValuePair<string, T>> values)
        {
            var entrys = values.Select(p => new HashEntry(p.Key, JsonConvert.SerializeObject(p.Value)));

            await _redis.Value.GetDatabase().HashSetAsync(key, entrys.ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static void AddHashList<T>(string key, IEnumerable<T> values, Func<T, string> getModelId)
        {
            var entrys = values.Select(p => new HashEntry(getModelId(p), JsonConvert.SerializeObject(p)));

            _redis.Value.GetDatabase().HashSet(key, entrys.ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static Task AddHashListAsync<T>(string key, IEnumerable<T> values, Func<T, string> getModelId)
        {
            var entrys = values.Select(p => new HashEntry(getModelId(p), JsonConvert.SerializeObject(p)));
            return _redis.Value.GetDatabase().HashSetAsync(key, entrys.ToArray(), flags: CommandFlags.PreferMaster);

        }


        public static IEnumerable<T> GetHashAllValue<T>(string key, bool fromMaster = true)
        {
    
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            HashEntry[] arr = _redis.Value.GetDatabase().HashGetAll(key, flags: flags);
            return arr.Where(p => !p.Value.IsNullOrEmpty).Select(p => JsonConvert.DeserializeObject<T>(p.Value));
        }

        public static async Task<IEnumerable<T>> GetHashAllValueAsync<T>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;

            HashEntry[] arr = await _redis.Value.GetDatabase().HashGetAllAsync(key, flags: flags);
            return arr.Where(p => !p.Value.IsNullOrEmpty).Select(p => JsonConvert.DeserializeObject<T>(p.Value));
        }

        public static async Task<IEnumerable<KeyValuePair<TKey, TValue>>> GetHashAllAsync<TKey, TValue>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;

            HashEntry[] arr = await _redis.Value.GetDatabase().HashGetAllAsync(key, flags: flags);
            return arr.Where(p => !p.Value.IsNullOrEmpty).Select(p =>
                new KeyValuePair<TKey, TValue>(JsonConvert.DeserializeObject<TKey>(p.Name),
                JsonConvert.DeserializeObject<TValue>(p.Value)));
            
        }

        public static string GetHashValue(string key, string hashkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().HashGet(key, hashkey, flags: flags);
        }

        public static async Task<string> GetHashValueAsync(string key, string hashkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return await (_redis.Value.GetDatabase().HashGetAsync(key, hashkey, flags:flags));

        }

        public static T GetHashValue<T>(string key, string hashkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            string result = _redis.Value.GetDatabase().HashGet(key, hashkey, flags: flags);
            if (string.IsNullOrEmpty(result))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public static async Task<T> GetHashValueAsync<T>(string key, string hashkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            string result = await _redis.Value.GetDatabase().HashGetAsync(key, hashkey, flags: flags);
            if (string.IsNullOrEmpty(result))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(result);
             

        }

        public static bool DeleteHashValue(string key, string hashkey)
        {
            return _redis.Value.GetDatabase().HashDelete(key, hashkey, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> DeleteHashValueAsync(string key, string hashkey)
        {
            return _redis.Value.GetDatabase().HashDeleteAsync(key, hashkey, flags: CommandFlags.PreferMaster);
        }

        public static Task DeleteHashValuesAsync(string key, IEnumerable<string> hashkeys)
        {
            return _redis.Value.GetDatabase().HashDeleteAsync(key, 
                hashkeys.Select(x => (RedisValue)x).ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static void DeleteHashValueNoWait(string key, string hashkey)
        {
            _redis.Value.GetDatabase().HashDelete(key, hashkey, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        #endregion


        #region zset类型操作

        public static bool AddZsetValue(string key, string zsetkey, double score)
        {
            bool ret = _redis.Value.GetDatabase().SortedSetAdd(key, zsetkey, score, flags: CommandFlags.PreferMaster);
            return ret;
        }


        public static async Task<bool> AddZsetValueAsync(String key, string zsetkey, double score)
        {
            return await _redis.Value.GetDatabase().SortedSetAddAsync(key, zsetkey, score, flags: CommandFlags.PreferMaster);
    
        }

        public static void AddZsetValueNoWait(String key, string zsetkey, double score)
        {
            _redis.Value.GetDatabase().HashSet(key, zsetkey, score, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }


        public static IEnumerable<KeyValuePair<string, double>> GetZsetAll(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            var arr = _redis.Value.GetDatabase().SortedSetRangeByRankWithScores(key, 0, -1, flags:flags);
            return arr.Select(p => new KeyValuePair<string, double>(p.Element, p.Score));
        }

        public static async Task<IEnumerable<KeyValuePair<string, double>>> GetZsetAllAsync(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            var arr = await _redis.Value.GetDatabase().SortedSetRangeByRankWithScoresAsync(key, 0, -1, flags: flags);
            return arr.Select(p => new KeyValuePair<string, double>(p.Element, p.Score));

        }


        public static async Task<IEnumerable<string>> GetZsetAllKeyAsync(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            var arr = await _redis.Value.GetDatabase().SortedSetRangeByRankWithScoresAsync(key, 0, -1, flags: flags);
            return arr.Select(p => p.Element.ToString());
        }

        public static double? GetZsetValue(string key, string zsetkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().SortedSetScore(key, zsetkey, flags:flags);
        }

        public static Task<double?> GetZsetValueAsync(string key, string zsetkey, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().SortedSetScoreAsync(key, zsetkey, flags:flags);

        }

        public static long DeleteZsetValueRange(string key, double minScore, double maxScore)
        {
            return _redis.Value.GetDatabase().SortedSetRemoveRangeByScore(key, minScore, maxScore, flags: CommandFlags.PreferMaster);
        }

        public static Task<long> DeleteZsetValueRangeAsync(string key, double minScore, double maxScore)
        {
            return _redis.Value.GetDatabase().SortedSetRemoveRangeByScoreAsync(key, minScore, maxScore, flags: CommandFlags.PreferMaster);
        }

        public static async Task<IEnumerable<string>> DeleteZsetReturnValueRangeAsync(string key, double minScore, double maxScore)
        {
            var deletedKey = await _redis.Value.GetDatabase().SortedSetRangeByScoreAsync(key, minScore, maxScore, flags: CommandFlags.PreferMaster);
            await _redis.Value.GetDatabase().SortedSetRemoveRangeByScoreAsync(key, minScore, maxScore, flags: CommandFlags.PreferMaster);
            return deletedKey.Select(x => x.ToString());
        }

        public static bool DeleteZsetValue(string key, string zsetkey)
        {
            return _redis.Value.GetDatabase().SortedSetRemove(key, zsetkey, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> DeleteZsetValueAsync(string key, string zsetkey)
        {
            return _redis.Value.GetDatabase().SortedSetRemoveAsync(key, zsetkey, flags: CommandFlags.PreferMaster);
        }

        public static void DeleteZsetValueNoWait(string key, string zsetkey)
        {
            _redis.Value.GetDatabase().SortedSetRemove(key, zsetkey, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        #endregion


        #region set类型操作

        public static long GetSetCount(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().SetLength(key, flags: flags);
        }
        public static Task<long> GetSetCountAsync(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().SetLengthAsync(key, flags: flags);
        }

        public static bool AddSetValue(string key, string value)
        {
            return _redis.Value.GetDatabase().SetAdd(key, value, flags: CommandFlags.PreferMaster);
        }

        public static async Task<bool> AddSetValueAsync(string key, string value)
        {
            return await _redis.Value.GetDatabase().SetAddAsync(key, value, flags: CommandFlags.PreferMaster);
        }


        public static bool AddSetValue<T>(String key, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            return _redis.Value.GetDatabase().SetAdd(key, json, flags: CommandFlags.PreferMaster);
        }

        public static Task<bool> AddSetValueAsync<T>(String key, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            return _redis.Value.GetDatabase().SetAddAsync(key, json, flags: CommandFlags.PreferMaster);
        }

        public static void AddSetValueNoWait<T>(String key, T t)
        {
            var json = JsonConvert.SerializeObject(t);
            _redis.Value.GetDatabase().SetAdd(key, json, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }


        public static void AddSetList<T>(string key, IEnumerable<T> list, Func<T, string> getModelId)
        {
            var all = list.Select(p => (RedisValue)getModelId(p));
            _redis.Value.GetDatabase().SetAdd(key, all.ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static Task AddSetListAsync<T>(string key, IEnumerable<T> list, Func<T, string> getModelId)
        {
           
            var all = list.Select(p => (RedisValue)getModelId(p));
            return _redis.Value.GetDatabase().SetAddAsync(key, all.ToArray(), flags: CommandFlags.PreferMaster);

        }

        public static async Task AddSetListAsync<T>(string key, IEnumerable<T> list)
        {
            var all = list.Select(p => (RedisValue)JsonConvert.SerializeObject(p));
            await _redis.Value.GetDatabase().SetAddAsync(key, all.ToArray(), flags: CommandFlags.PreferMaster);
        }

        public static Task AddSetListAsync(string key, IEnumerable<string> list)
        {
            var all = list.Select(p => (RedisValue)p);
            return _redis.Value.GetDatabase().SetAddAsync(key, all.ToArray(), CommandFlags.PreferMaster);
        }

        public static IEnumerable<T> GetSetAllValue<T>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            RedisValue[] arr = _redis.Value.GetDatabase().SetMembers(key, flags: flags);
            return arr.Select(p => JsonConvert.DeserializeObject<T>(p.ToString()));
        }



        public static async Task<IEnumerable<T>> GetSetAllValueAsync<T>(string key, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            RedisValue[] arr = await _redis.Value.GetDatabase().SetMembersAsync(key, flags: flags);
            return arr.Select(p => JsonConvert.DeserializeObject<T>(p.ToString()));
            
        }


        public static bool IsSetContains(string key, string value, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return _redis.Value.GetDatabase().SetContains(key, value, flags:flags);
        }

        public static async Task<bool> IsSetContainsAsync(string key, string value, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            return await _redis.Value.GetDatabase().SetContainsAsync(key, value, flags: flags);

        }


        public static bool IsSetContains<T>(string key, T value, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            string strvalue = JsonConvert.SerializeObject(value);
            return _redis.Value.GetDatabase().SetContains(key, strvalue, flags: flags);

        }

        public static async Task<bool> IsSetContainsAsync<T>(string key, T value, bool fromMaster = true)
        {
            CommandFlags flags = fromMaster ? CommandFlags.PreferMaster : CommandFlags.PreferSlave;
            string strvalue = JsonConvert.SerializeObject(value);
            return await _redis.Value.GetDatabase().SetContainsAsync(key, strvalue, flags: flags);


        }

        public static bool DeleteSetValue(string key, string value)
        {
            return _redis.Value.GetDatabase().SetRemove(key, value, flags: CommandFlags.PreferSlave);
        }

        public static Task<bool> DeleteSetValueAsync(string key, string value)
        {
            return _redis.Value.GetDatabase().SetRemoveAsync(key, value, flags: CommandFlags.PreferSlave);
        }

        public static void DeleteSetValueNoWait(string key, string value)
        {
            _redis.Value.GetDatabase().SetRemove(key, value, flags: CommandFlags.FireAndForget | CommandFlags.PreferMaster);
        }

        #endregion
    }
}
