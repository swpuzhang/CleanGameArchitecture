using RoomMatch.ViewModels;
using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.Buses;
using RoomMatch.Domain.ProcessCommands;
using RoomMatch.Domain.Entitys;
using RoomMatch.Manager;

namespace RoomMatch.Application.Services
{
    public class RoomMatchService : IRoomMatchService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly RoomManager _roomManager;
        private readonly MatchManager _matchingManager;
        public RoomMatchService(IMapper mapper, IMediatorHandler bus,
            RoomManager roomManager, MatchManager matchingManager)
        {
            _mapper = mapper;
            _bus = bus;
            _roomManager = roomManager;
            _matchingManager = matchingManager;
        }

        public async Task<BodyResponse<MatchingResponseVM>> Playnow(long id)
        {
            BodyResponse<SangongMatchingResponseInfo> response = await _bus.SendCommand(new SangongPlaynowCommand(id));
            return response.MapResponse<MatchingResponseVM>(_mapper);
        }

        public void SynGameRooms(SyncGameRoomMqCommand command)
        {
            _roomManager.SyncRooms(command.GameKey, command.MatchingGroup, command.SyncInfo);
        }

        public void OnJoinGameRoom(JoinGameRoomMqEvent joinEvent)
        {
            _ = _matchingManager.OnJoinGame(joinEvent.Id, joinEvent.GameKey, joinEvent.RoomId,
                joinEvent.Blind, joinEvent.UserCount, joinEvent.MatchingGroup);
        }

        public void OnLeaveGameRoom(LeaveGameRoomMqEvent leaveEvent)
        {
            _ = _matchingManager.OnLeaveGame(leaveEvent.Id, leaveEvent.GameKey, leaveEvent.RoomId,
                leaveEvent.Blind, leaveEvent.UserCount, leaveEvent.MatchingGroup);
        }

        public Task<BodyResponse<NullBody>> OnUserApplySit(UserApplySitMqCommand sitcmd)
        {
            return _matchingManager.OnUserApplySit(sitcmd.Id, sitcmd.GameKey, sitcmd.Blind, sitcmd.RoomId);
        }

        public void OnUserSiteFailed(UserSitFailedMqEvent sitEvent)
        {
            _ = _matchingManager.OnSiteFailed(sitEvent.Id, sitEvent.GameKey, sitEvent.RoomId, sitEvent.MatchingGroup);
        }

        public Task<BodyResponse<GetBlindRoomListResponse>> GetBlindRoomList(long id)
        {
            return Task.FromResult(new BodyResponse<GetBlindRoomListResponse>(StatusCodeDefines.Success, null,
                _roomManager.GetBindRoomList()));

        }

        public async Task<BodyResponse<MatchingResponseVM>> BlindMatching(long id, long blind)
        {
            BodyResponse<SangongMatchingResponseInfo> response =
                await _bus.SendCommand(new BlindMatchingCommand(id, blind));
            return response.MapResponse<MatchingResponseVM>(_mapper);
        }

    }
}
