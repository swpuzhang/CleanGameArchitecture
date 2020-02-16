using Commons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMatch.Domain.Entitys
{
    public enum MatchingStatus
    {
        Success = 0,
        IsMathing = 1
    }
    public class UserRoomInfo : UserEntity
    {
        [JsonConstructor]
        public UserRoomInfo(long id, string roomId, string gameKey, long blind, MatchingStatus status)
        {
            Id = id;
            RoomId = roomId;
            GameKey = gameKey;
            Blind = blind;
            Status = status;
        }

        public string RoomId { get; private set; }
        public string GameKey { get; private set; }

        public long Blind { get; private set; }

        public MatchingStatus Status { get; private set; }
    }
}
