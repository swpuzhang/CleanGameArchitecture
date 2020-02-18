using Commons.Enums;
using RoomMatch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.ViewModels
{
    /// <summary>
    /// 获取房间列表
    /// </summary>
    public class GetBlindRoomListResponseVm
    {
        public List<BlindRoomList> RoomList { get; set; }
    }

    /// <summary>
    ///匹配房间结果
    /// </summary>
    public class RoomMatchResponseVm
    {
        public RoomMatchResponseVm(string roomId, long blind, string gameKey)
        {
            RoomId = roomId;
            Blind = blind;
            GameKey = gameKey;
        }

        public string RoomId { get; set; }
        public long Blind { get; set; }
        /// <summary>
        /// 请求gameserver的Key
        /// </summary>
        public string GameKey { get; set; }
    }
}
