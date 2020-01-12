using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Commons.Enums;

namespace Commons.Models
{
    public class NullBody
    {

    }

    public class WrappedResponse<TBody> where TBody : class
    {
        public WrappedResponse()
        {

        }

        public WrappedResponse(ResponseStatus responseStatus, List<string> errorInfos = null, TBody body = null)
        {
            ResponseStatus = responseStatus;
            ErrorInfos = errorInfos;
            Body = body;
        }

        public WrappedResponse<V> MapResponse<V>(IMapper mapper) where V : class
        {
            return new WrappedResponse<V>(ResponseStatus, ErrorInfos, mapper.Map<V>(Body));
        }

        public ResponseStatus ResponseStatus { get; private set; }
        public List<string> ErrorInfos { get; private set; }
        public TBody Body { get; set; }
    }
}
