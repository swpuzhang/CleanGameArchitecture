using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSGateway.ViewModels
{
    /// <summary>
    /// websocket建立连接后第一个需要发的消息，验证玩家，并且创建玩家对象，之后的消息不需要再加token
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 账号登录时的token
        /// </summary>
        public string Token { get; set; }
    }
}
