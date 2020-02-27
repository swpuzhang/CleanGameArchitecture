using Game.Domain.Entitys;
using Game.ViewModels;
using Commons.Buses.ProcessBus;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Domain.ProcessCommands
{
    public class GameCommand : ProcessCommand<WrappedResponse<GameResponseVm>>
    {
        public GameInfo Info { get; private set; }
        public GameCommand(GameInfo info)
        {
            Info = info;
        }
    }
}
