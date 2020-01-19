using Account.Application.Services;
using CommonMessages.MqCmds;
using CommonMessages.MqEvents;
using Commons.Enums;
using Commons.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.MqConsumers
{
    public class AccountMqConsumer:
        IConsumer<GetAccountInfoMqCmd>,
        IConsumer<GetAccountBaseInfoMqCmd>,
        IConsumer<FinishedRegisterRewardMqEvent>,
        IConsumer<GetIdByPlatformMqCmd>
    {
        private readonly IAccountService _service;

        public AccountMqConsumer(IAccountService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<GetAccountInfoMqCmd> context)
        {
            var response = await _service.GetSelfAccount(context.Message.Id);
            if (response.ResponseStatus != ResponseStatus.Success)
            {
                await context.RespondAsync<WrappedResponse<GetAccountInfoMqResponse>>(new WrappedResponse<GetAccountInfoMqResponse>(response.ResponseStatus,
                    null, null));
                return;
            }
            await context.RespondAsync<WrappedResponse<GetAccountInfoMqResponse>>(new WrappedResponse<GetAccountInfoMqResponse>(ResponseStatus.Success, null, new GetAccountInfoMqResponse(context.Message.Id,
                response.Body.PlatformAccount, response.Body.UserName, response.Body.Sex, response.Body.HeadUrl,
                new GameInfoMq(response.Body.GameInfo.GameTimes, response.Body.GameInfo.WinTimes, response.Body.GameInfo.MaxWinCoins),
                new LevelInfoMq(response.Body.LevelInfo.CurLevel, response.Body.LevelInfo.CurExp, response.Body.LevelInfo.NeedExp),
                response.Body.MoneyInfo.CurCoins, response.Body.MoneyInfo.CurDiamonds)));
        }

        public async Task Consume(ConsumeContext<GetAccountBaseInfoMqCmd> context)
        {
            var response = await _service.GetAccountBaseInfo(context.Message.Id);
            await context.RespondAsync(response);
        }

        public Task Consume(ConsumeContext<FinishedRegisterRewardMqEvent> context)
        {
            _service.FinishRegisterReward(context.Message.Id);
            return Task.CompletedTask;
        }

        public async Task Consume(ConsumeContext<GetIdByPlatformMqCmd> context)
        {
            var response = await _service.GetIdByPlatform(context.Message.PlatformAccount, context.Message.Type);

            await context.RespondAsync(response);
        }
    }
}
