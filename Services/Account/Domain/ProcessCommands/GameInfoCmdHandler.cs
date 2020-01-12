using Account.Domain.Entitys;
using Commons.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commons.Tools.KeyGen;
using Account.Infrastruct.Repository;
using Account.Domain.Manager;
using Commons.Enums;

namespace Account.Domain.ProcessCommands
{
    public class GetGameInfoCmdHandler : IRequestHandler<GetGameInfoCommand, WrappedResponse<GameInfo>>
    {
        private readonly IGameInfoRepository _gameRepository;
        private readonly IAllRedisRepository _redisRepository;
      
        public GetGameInfoCmdHandler(IGameInfoRepository rep,
            IAllRedisRepository redisRepository)
        {
            _gameRepository = rep;
            _redisRepository = redisRepository;
        }
        public async Task<WrappedResponse<GameInfo>> Handle(GetGameInfoCommand request, CancellationToken cancellationToken)
        {

            GameInfo gameinfo = await _redisRepository.GetGameInfo(request.Id);
            if (gameinfo == null)
            {
                using var loker = _redisRepository.Locker(KeyGenTool.GenUserKey(request.Id, GameInfo.ClassName));
                loker.Lock();
                gameinfo = await _gameRepository.FindAndAdd(request.Id, new GameInfo(request.Id, 0, 0, 0));
                _ = _redisRepository.SetGameInfo(gameinfo);
            }

            WrappedResponse<GameInfo> response = new WrappedResponse<GameInfo>(ResponseStatus.Success,
                null, gameinfo);

            return response;
        }
    }
}
