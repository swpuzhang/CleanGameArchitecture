using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Threading;
using System.Threading.Tasks;

namespace Commons.Db.Redis
{
    public interface IRedisRepository
    {
        ReidsLockGuard Locker(string key);
    }

    public class ReidsLockGuard :  IDisposable
    {
        private readonly string _key;
        private readonly string _ownValue;
        public ReidsLockGuard(string key)
        {
            _key = key + "_l_o_k_e_r";
            _ownValue = Guid.NewGuid().ToString();
        }
        public void Lock(int ms = 5000)
        {
            while (!RedisOpt.LockTake(_key, _ownValue, TimeSpan.FromMilliseconds(ms)))
            {
                Thread.Sleep(2);
            }
      
        }
        public bool TryLock(int ms = 5000)
        {
            return RedisOpt.LockTake(_key, _ownValue, TimeSpan.FromMilliseconds(ms));
   
        
        }

        public async Task LockAsync(int ms = 5000)
        {
            while (!await RedisOpt.LockTakeAsync(_key, _ownValue, TimeSpan.FromMilliseconds(ms)))
            {
                Thread.Sleep(2);
            }

        }
        public Task<bool> TryLockAsync(int ms = 5000)
        {
            return RedisOpt.LockTakeAsync(_key, _ownValue, TimeSpan.FromMilliseconds(ms));
        }

        public void Dispose()
        {

            RedisOpt.LockRelease(_key, _ownValue);
            
        }
    }
    public class RedisRepository : IRedisRepository
    {

        public RedisRepository()
        {

        }

        public ReidsLockGuard Locker(string key)
        {
            return new ReidsLockGuard(key);
        }
    }
}
