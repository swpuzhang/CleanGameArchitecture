using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Db.Mongodb
{
    public interface IMongoUserRepository<T>
    {
        void Add(T obj);
        T GetById(Int64 id);
        IEnumerable<T> GetAll();
        void Replace(T obj);
        void Remove(Int64 id);
        Task AddAsync(T obj);
        Task<T> GetByIdAsync(Int64 id);
        Task<IEnumerable<T>> GetAllAsync();
        Task ReplaceAsync(T obj);
        Task ReplaceAndAddAsync(T obj);
        Task UpdateAsync(T obj);
        Task RemoveAsync(Int64 id);
        Task<T> FindAndAdd(long id, T info);

    }
}
