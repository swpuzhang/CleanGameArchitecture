using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Db.Mongodb
{
    public class MongoConfigRepository<T> : IMongoConfigRepository<T>
    {
        protected readonly IMongoCollection<T> _dbCol;
        public MongoConfigRepository(IMongoCollection<T> dbCol)
        {
            _dbCol = dbCol;
        }

        public IEnumerable<T> LoadMultiConfig()
        {
         
            var configs = _dbCol.Find<T>(x => true);//.Project(Builders<LevelConfig>.
                                                             //Projection.Exclude("_id")).ToList()
            return configs.Project<T>(Builders<T>.Projection.Exclude("_id")).ToEnumerable();
        }

        public T LoadConfig()
        {
            var configs = _dbCol.Find<T>(x => true);//.Project(Builders<LevelConfig>.
                                                    //Projection.Exclude("_id")).ToList();

            return configs.Project<T>(Builders<T>.Projection.Exclude("_id")).FirstOrDefault();
        }
    }
}
