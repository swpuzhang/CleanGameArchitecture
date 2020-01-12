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
    public class GetLevelInfoCmdHandler : IRequestHandler<GetLevelInfoCommand, WrappedResponse<LevelInfo>>
    {
        private readonly ILevelInfoRepository _levelRepository;
        private readonly IAllRedisRepository _redisRepository;
      
        public GetLevelInfoCmdHandler(ILevelInfoRepository rep,
            IAllRedisRepository redisRepository)
        {
            _levelRepository = rep;
            _redisRepository = redisRepository;
        }
        public async Task<WrappedResponse<LevelInfo>> Handle(GetLevelInfoCommand request, CancellationToken cancellationToken)
        {
            LevelInfo levelinfo = await _redisRepository.GetLevelInfo(request.Id);
            if (levelinfo == null)
            {
                //从数据库中获取
                using var loker = _redisRepository.Locker(KeyGenTool.GenUserKey(request.Id, LevelInfo.ClassName));
                loker.Lock();
                levelinfo = await _levelRepository.FindAndAdd(request.Id,
                        new LevelInfo(request.Id, 1, 0, LevelManager.GetNeedExp(1)));
                _ = _redisRepository.SetLevelInfo(levelinfo);
            }

            WrappedResponse<LevelInfo> response = new WrappedResponse<LevelInfo>(ResponseStatus.Success,
                null, levelinfo);

            return response;
        }
    }
}
