using Commons.Db.Redis;
using Commons.Enums;
using Commons.Models;
using Commons.Tools.KeyGen;
using GameMessages.MqCmds;
using Microsoft.Extensions.Configuration;
using RoomMatch.Domain.Entitys;
using RoomMatch.Domain.Models;
using RoomMatch.Infrastruct.Repository;
using RoomMatch.ViewModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.Manager
{
    public static class MatchManager
    {
        private static List<CoinsRangeConfig> _coinsRangeCfg;
        private static ICoinsRangeConfigRepository _configRespository;
        private static IRoomMatchRedisRepository _redis;
        public static string matchingGroup;

        public static void Init(IConfiguration config, ICoinsRangeConfigRepository configRespository,
            IRoomMatchRedisRepository redis)
        {
            _configRespository = configRespository;
            _redis = redis;
            matchingGroup = config["MatchingGroup"];
            LoadCoinsRangeConfig();
        }

        public static void LoadCoinsRangeConfig()
        {
            _coinsRangeCfg = _configRespository.LoadMultiConfig().ToList();
        }

        public static bool GetBlindFromCoins(long coins, out long blind)

        {
            blind = 0;
            foreach (var oneCfg in _coinsRangeCfg)
            {
                if (coins >= oneCfg.CoinsBegin && coins < (oneCfg.CoinsEnd == -1 ? long.MaxValue : oneCfg.CoinsEnd))
                {
                    blind = oneCfg.Blind;
                    return true;
                }
            }
            return false;
        }

        public static async Task<WrappedResponse<RoomMatchResponseVm>> MatchRoom(long id, long blind, string curRoomId)
        {

            //获取这个玩家的redis锁
            using var locker = _redis.Locker(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo)));

            if (!await locker.TryLockAsync())
            {
                return new WrappedResponse<RoomMatchResponseVm>(ResponseStatus.IsMatching, null, null);
            }

            //查询redis是否有这个玩家
            RoomInfo roomInfo = null;
            var userRoomInfo = await _redis.GetUserRoomInfo(id);
            if (userRoomInfo != null)
            {
                roomInfo = new RoomInfo(userRoomInfo.RoomId, 0, userRoomInfo.GameKey, userRoomInfo.Blind);
            }
            else
            {
                roomInfo = await RoomManager.GetRoom(blind);
            }
            try
            {
                WrappedResponse<JoinGameRoomMqResponse> roomResponse =
                    await RoomManager.SendToGameRoom<JoinGameRoomMqCmd, WrappedResponse<JoinGameRoomMqResponse>>
                    (roomInfo.GameKey, new JoinGameRoomMqCmd(id, roomInfo.RoomId, roomInfo.GameKey));

                if (roomResponse.ResponseStatus == ResponseStatus.Success)
                {
                    _ = _redis.SetUserRoomInfo(new UserRoomInfo(id, roomInfo.RoomId, roomInfo.GameKey, blind, MatchingStatus.Success));
                    return new WrappedResponse<RoomMatchResponseVm>(ResponseStatus.Success, null,
                        new RoomMatchResponseVm(roomInfo.RoomId, roomInfo.Blind, roomInfo.GameKey));
                }
                else
                {
                    if (roomResponse.Body != null)
                    {
                        RoomInfo newRoomInfo = new RoomInfo(roomResponse.Body.RoomId, roomResponse.Body.UserCount,
                            roomResponse.Body.GameKey, roomResponse.Body.Blind);
                        RoomManager.UpdateRoom(newRoomInfo);
                    }

                    _ = _redis.DeleteUserRoomInfo(id);
                    return new WrappedResponse<RoomMatchResponseVm>(roomResponse.ResponseStatus, roomResponse.ErrorInfos, null);
                }
            }

            catch
            {
                _ = _redis.DeleteUserRoomInfo(id);
                Log.Error($"user {id} join room {roomInfo.RoomId} error");
                return new WrappedResponse<RoomMatchResponseVm>(ResponseStatus.BusError, null, null);
            }

        }

        /*public static Task OnJoinGame(long id, string gameKey, string roomId, long blind, int userCount, string group)
        {
            if (group != matchingGroup)
            {
                return;
            }

            using (var locker = _redis.Locker(KeyGenHelper.GenUserKey(id, UserRoomInfo.className)))
            {
                await locker.LockAsync();
                _ = _redis.SetUserRoomInfo(new UserRoomInfo(id, roomId, gameKey, blind, MatchingStatus.Success));
                
            }
                
            _roomManager.OnUserCountChange(gameKey, roomId, blind, userCount);
            return Task.CompletedTask;
        }*/

        public static async Task OnLeaveGame(long id, string gameKey, string roomId, long blind, int userCount, string group)
        {
            if (group != matchingGroup)
            {
                return;
            }

            using (var locker = _redis.Locker(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo))))
            {
                await locker.LockAsync();
                _ = _redis.DeleteUserRoomInfo(id);
            }
            RoomManager.OnUserCountChange(gameKey, roomId, -1);
        }

        public static async Task<WrappedResponse<NullBody>> OnUserApplySit(long id, string gameKey, Int64 blind, string roomId)
        {
            using var locker = _redis.Locker(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo)));

            await locker.LockAsync();
            var userRoomInfo = await _redis.GetUserRoomInfo(id);
            if (userRoomInfo != null)
            {
                return new WrappedResponse<NullBody>(ResponseStatus.Error, new List<string>() { "user already in room " });
            }
            if (!RoomManager.JoinOneRoom(gameKey, roomId))
            {
                return new WrappedResponse<NullBody>(ResponseStatus.Error, new List<string>() { "room is full " });
            }
            _ = _redis.SetUserRoomInfo(new UserRoomInfo(id, roomId, gameKey, blind, MatchingStatus.Success));
            return new WrappedResponse<NullBody>(ResponseStatus.Success, null);

        }

        public static async Task OnSiteFailed(long id, string gameKey, string roomId, string group)
        {

            if (group != matchingGroup)
            {
                return;
            }
            using var locker = _redis.Locker(KeyGenTool.GenUserKey(id, nameof(UserRoomInfo)));
            await locker.LockAsync();
            _ = _redis.DeleteUserRoomInfo(id);
            RoomManager.JoinOneRoomFailed(gameKey, roomId);

        }

    }
}
