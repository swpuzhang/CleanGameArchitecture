using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Application.Services;
using Game.ViewModels;
using Commons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Game.Controllers
{
    /// <summary>
    /// 账号相关操作
    /// </summary>
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;
        public GameController(IGameService service)
        {
            _service = service;
        }

        [HttpGet]
        public  WrappedResponse<GameResponseVm> Test()
        {
            return new WrappedResponse<GameResponseVm>();
        }

    }
}