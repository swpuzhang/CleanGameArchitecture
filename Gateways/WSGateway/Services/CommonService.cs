
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Tools.Encryption;
using Commons.Enums;

namespace WSGateWay.Services
{
    public class CommonService : ICommonService
    {
        public KeyValuePair<bool, long> TokenValidation(string token)
        {

            var status = TokenTool.ParseToken(token, out var id);
            if (status != ResponseStatus.Success)
            {
                return new KeyValuePair<bool, long>(false, id);
            }

            return new KeyValuePair<bool, long>(true, id);
        }
    }
}
