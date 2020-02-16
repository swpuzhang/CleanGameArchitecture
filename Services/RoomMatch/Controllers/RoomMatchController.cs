using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoomMatch.Application.Services;
using RoomMatch.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Commons.Threading;

namespace RoomMatch.Controllers
{
    /// <summary>
    /// 房间列表和房间匹配
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RoomMatchController : ControllerBase
    {
        private readonly IRoomMatchService _service;

        public RoomMatchController(IRoomMatchService service)
        {
            _service = service;
        }

        /// <summary>
        /// Playnow 接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<WrappedResponse<RoomMatchResponseVm>> PlayNow([FromHeader]long id)
        {
            var response = await OneThreadSynchronizationContext.UserRequest(id, _service.Playnow);
            return response;
        }

        [HttpGet]
        public async Task<WrappedResponse<GetBlindRoomListResponseVm>> GetBlindRoomList([FromHeader]long id)
        {
            var response = await OneThreadSynchronizationContext
                .UserRequest<long, WrappedResponse<GetBlindRoomListResponseVm>>
                    (id, _service.GetBlindRoomList);
            return response;
        }

        [HttpGet]
        public async Task<WrappedResponse<RoomMatchResponseVm>> BlindMatching([FromHeader]long id, long blind)
        {

            return await OneThreadSynchronizationContext.UserRequest(id, blind, _service.BlindMatching);
        }
    }
}