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

    /// <summary>
    /// 回复给客户端Response
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
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

        /// <summary>
        /// 响应状态码
        /// </summary>
        public ResponseStatus ResponseStatus { get; private set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public List<string> ErrorInfos { get; private set; }
        /// <summary>
        /// 具体的响应内容
        /// </summary>
        public TBody Body { get; set; }
    }
}
