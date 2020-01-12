using System;
using System.Collections.Generic;
using System.Text;
using Commons.Enums;
using Commons.Tools.Time;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Commons.Tools.Encryption
{
    public class UserToken
    {
        public long Id { get; set; }
        public long GenTime { get; set; }
    }

    public static class TokenTool
    {
        public static string GenToken(long uid)
        {
            UserToken token = new UserToken
            {
                Id = uid,
                GenTime = DateTime.Now.ToTimeStamp()
            };
            string inputstr = JsonConvert.SerializeObject(token);

            return Secret.EncryptAES(inputstr);
        }

        public static ResponseStatus CheckToken(string str, long id, long expireSec = 3600 * 24)
        {

            try
            {
                string decStr = Secret.DecryptAES(str);
                UserToken token = JsonConvert.DeserializeObject<UserToken>(decStr);
                if (token == null || token.Id != id)
                {
                    return ResponseStatus.TokenError;
                }
                long tnow = DateTime.Now.ToTimeStamp();
                if (tnow - token.GenTime >= expireSec)
                {
                    return ResponseStatus.TokenExpiredPleaseRelogin;
                }
                return ResponseStatus.Success;
            }
            catch (Exception)
            {
                return ResponseStatus.TokenError;
            }

        }

        public static ResponseStatus ParseToken(string str, out long id, long expireSec = 3600 * 24)
        {
            id = 0;
            try
            {
                string decStr = Secret.DecryptAES(str);
                UserToken token = JsonConvert.DeserializeObject<UserToken>(decStr);
                if (token == null)
                {
                    return ResponseStatus.TokenError;
                }
                long tnow = DateTime.Now.ToTimeStamp();
                if (tnow - token.GenTime >= expireSec)
                {
                    return ResponseStatus.TokenExpiredPleaseRelogin;
                }
                id = token.Id;
                return ResponseStatus.Success;
            }
            catch (Exception)
            {
                return ResponseStatus.TokenError;
            }

        }

    }
}
