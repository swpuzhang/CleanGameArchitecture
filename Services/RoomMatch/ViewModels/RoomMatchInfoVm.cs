using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMatch.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomMatchResponseVm
    {
        public string RoomId { get; set; }
        public long Blind { get; set; }
        /// <summary>
        /// 请求gameserver的Key
        /// </summary>
        public string GameKey { get; set; }
    }
}
