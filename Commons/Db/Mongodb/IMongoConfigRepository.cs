using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Db.Mongodb
{
    public interface IMongoConfigRepository<T>
    {
        public T LoadConfig();
        public IEnumerable<T> LoadMultiConfig();
    }
}
