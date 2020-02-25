using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSGateWay.Services
{
    public interface ICommonService
    {
        KeyValuePair<bool, long> TokenValidation(string token);
    }
}
