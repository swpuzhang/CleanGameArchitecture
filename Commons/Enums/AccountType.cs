using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Commons.Enums
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// 游客
        /// </summary>
       [Description("游客")]
        Tourist = 0,
        /// <summary>
        /// Facebook
        /// </summary>
        [Description("Facebook")]
        Facebook = 1,

        /// <summary>
        /// Twitter
        /// </summary>
        [Description("Twitter")]
        Twitter = 2
    }
}
