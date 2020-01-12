using Money.ViewModels;
using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.Buses;
using Money.Domain.ProcessCommands;
using Money.Domain.Entitys;
using Messages.MqCmds;
using Commons.Enums;

namespace Money.Application.Services
{
    public class MoneyService : IMoneyService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        public MoneyService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<WrappedResponse<MoneyMqResponse>> GetMoney(long id)
        {
            var info = await _bus.SendCommand(new GetMoneyCommand(id));
            if (info.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<MoneyMqResponse>(info.ResponseStatus, null, null);
            }
            var moneyResponse = _mapper.Map<MoneyMqResponse>(info.Body);
            return new WrappedResponse<MoneyMqResponse>(ResponseStatus.Success, null, moneyResponse);
        }

        public async Task<WrappedResponse<MoneyMqResponse>> AddMoney(long id, long addCoins, long addCarry)
        {
            var moneyInfo = await _bus.SendCommand(new AddMoneyCommand(id, addCoins, addCarry));
            if (moneyInfo.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<MoneyMqResponse>(moneyInfo.ResponseStatus, null, null);
            }
            var moneyResponse = _mapper.Map<MoneyMqResponse>(moneyInfo.Body);
            return new WrappedResponse<MoneyMqResponse>(ResponseStatus.Success, null, moneyResponse);
        }
    }
}
