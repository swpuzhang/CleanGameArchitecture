using Account.ViewModels;
using AutoMapper;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Commons.Buses.ProcessBus;
using Commons.Buses;
using Account.Domain.ProcessCommands;
using Account.Domain.Entitys;
using CommonMessages.MqCmds;
using Commons.Enums;
using Account.Domain.ProcessEvents;

namespace Account.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        public AccountService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }
        public async Task<WrappedResponse<AccountResponseVm>> Login(AccountInfoVm accountInfo)
        {
            var response = await _bus.SendCommand(new LoginCommand(_mapper.Map<AccountInfo>(accountInfo)));
            AccountResponseVm responseVM = _mapper.Map<AccountResponseVm>(response.Body);
            return new WrappedResponse<AccountResponseVm>(response.ResponseStatus, response.ErrorInfos, responseVM);
        }

        public  Task<WrappedResponse<AccountDetailVm>> GetSelfAccount(long id)
        {
            return  _bus.SendCommand(new GetSelfAccountCommand(id));
        }

        public  Task<WrappedResponse<OtherAccountDetailVm>> GetOtherAccount(long id, long otherId)
        {
            return _bus.SendCommand(new GetOtherAccountCommand(id, otherId));
            
        }

        public async Task<WrappedResponse<GetAccountBaseInfoMqResponse>> GetAccountBaseInfo(long id)
        {
            var response = await _bus.SendCommand(new GetAccountBaseInfoCommand(id));
            if (response.ResponseStatus != ResponseStatus.Success)
            {
                return new WrappedResponse<GetAccountBaseInfoMqResponse>(response.ResponseStatus, response.ErrorInfos);
            }
            return new WrappedResponse<GetAccountBaseInfoMqResponse>(ResponseStatus.Success, null,
                _mapper.Map<GetAccountBaseInfoMqResponse>(response.Body));
        }

        public void FinishRegisterReward(long id)
        {
            _bus.RaiseEvent(new FinishRegisterRewardEvent(id));
        }

        public Task<WrappedResponse<GetIdByPlatformMqResponse>> GetIdByPlatform(string platformAccount, int type)
        {
            return _bus.SendCommand(new GetIdByPlatformCommand(platformAccount, type));
        }
    }
}
