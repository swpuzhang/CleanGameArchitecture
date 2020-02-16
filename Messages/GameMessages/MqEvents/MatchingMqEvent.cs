using System;
using System.Collections.Generic;
using System.Text;

namespace GameMessages.MqEvents
{
    public class JoinGameRoomMqEvent
    {
        public JoinGameRoomMqEvent(long id, string roomId, string gameKey, int userCount, long blind, string matchingGroup)
        {
            Id = id;
            RoomId = roomId;
            GameKey = gameKey;
            UserCount = userCount;
            Blind = blind;
            MatchingGroup = matchingGroup;
        }

        public long Id { get; private set; }
        public string RoomId { get; private set; }

        public string GameKey { get; private set; }

        public int UserCount { get; private set; }

        public long Blind { get; private set; }

        public string MatchingGroup { get; private set; }
    }

    public class LeaveGameRoomMqEvent
    {
        public LeaveGameRoomMqEvent(long id, string roomId, string gameKey, int userCount, long blind, string matchingGroup)
        {
            Id = id;
            RoomId = roomId;
            GameKey = gameKey;
            UserCount = userCount;
            Blind = blind;
            MatchingGroup = matchingGroup;
        }

        public long Id { get; private set; }

        public string RoomId { get; private set; }

        public string GameKey { get; private set; }

        public int UserCount { get; private set; }

        public long Blind { get; private set; }

        public string MatchingGroup { get; private set; }
    }

    public class MatchingStartedMqEvent
    {
        public MatchingStartedMqEvent(string matchingGroup)
        {
            MatchingGroup = matchingGroup;
        }

        public string MatchingGroup { get; private set; }
    }

    public class MatchingStartedEvent
    {
        public MatchingStartedEvent(string matchingGroup)
        {
            MatchingGroup = matchingGroup;
        }

        public string MatchingGroup { get; private set; }

    }

    public class UserSitFailedMqEvent
    {
        public UserSitFailedMqEvent(long id, string roomId, string gameKey, string matchingGroup)
        {
            Id = id;
            RoomId = roomId;
            GameKey = gameKey;
            MatchingGroup = matchingGroup;
        }

        public long Id { get; private set; }
        public string RoomId { get; private set; }

        public string GameKey { get; private set; }

        public string MatchingGroup { get; private set; }
    }
}
