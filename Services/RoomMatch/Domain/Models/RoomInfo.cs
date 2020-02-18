using GameMessages.MqCmds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.Domain.Models
{
    public class RoomInfo : IComparable<RoomInfo>
    {
        public const int MAX_USER_NUM = 7;
        public RoomInfo(string roomId, int userCount, string gameKey, long blind)
        {
            RoomId = roomId;
            UserCount = userCount;
            GameKey = gameKey;
            Blind = blind;
        }
        public void UpdateUserCount(int count)
        {
            UserCount = count;
        }
        public void AddUserCount(int addCount)
        {
            UserCount += addCount;
        }
        public string RoomId { get; private set; }

        public int UserCount { get; private set; }

        public string GameKey { get; private set; }

        public long Blind { get; private set; }

        public bool IsFull() => UserCount == MAX_USER_NUM;

        public bool IsEmpty() => UserCount == 0;

        public int CompareTo(RoomInfo other)
        {
            if (UserCount == other.UserCount)
            {
                return RoomId.CompareTo(other.RoomId);
            }
            return -UserCount.CompareTo(other.UserCount);

        }
    }

    /// <summary>
    /// 房间列表结果
    /// </summary>
    public class BlindRoomList
    {
        public BlindRoomList()
        {
        }

        public BlindRoomList(RoomTypes roomType, long bind, long minCarry, long maxCarry, long minCoins, long maxCoins)
        {
            RoomType = roomType;
            Bind = bind;
            MinCarry = minCarry;
            MaxCarry = maxCarry;
            MinCoins = minCoins;
            MaxCoins = maxCoins;
        }
        /// <summary>
        /// 房间类型 0 体力场, 1初2中3高
        /// </summary>
        public RoomTypes RoomType { get; set; }
        public long Bind { get; set; }
        public long MinCarry { get; set; }
        public long MaxCarry { get; set; }
        /// <summary>
        /// 最小准入
        /// </summary>
        public long MinCoins { get; set; }
        /// <summary>
        /// 最大准入， 如果最大准入和最小准入相等， 那么就没有最大准入限制
        /// </summary>
        public long MaxCoins { get; set; }
    }
}
