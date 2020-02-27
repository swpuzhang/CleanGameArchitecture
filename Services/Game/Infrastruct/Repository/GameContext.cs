using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Domain.Entitys;
using Commons.Db.Mongodb;
using Commons.DiIoc;
using MongoDB.Driver;

namespace Game.Infrastruct.Repository
{
    public interface IGameContext : IDependency
    {
        public IMongoCollection<GameInfo> GameInfos { get;}
    }
    public class GameContext : MongoContext, IGameContext
    {
        public IMongoCollection<GameInfo> GameInfos { get; }
       
        public GameContext(IMongoSettings settings) : base(settings)
        {
            GameInfos = base._database.GetCollection<GameInfo>(typeof(GameInfo).Name);
        }
    }
}
