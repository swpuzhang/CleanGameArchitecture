using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Commons.Extenssions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Serilog;
using CommonMessages.MqCmds;
using GameMessages.MqCmds;
using Commons.Models;
using Commons.Buses.MqBus;
using Commons.Enums;
using Commons.DiIoc;

namespace Commons.Domain.Managers
{
    public class MqManager : ISingletonDependency
    {
        public readonly IBusControl _bus;
        //public static string accountUrl;
        //public static string moneyUrl;
        public readonly IRequestClient<GetAccountInfoMqCmd> _accountClient;
        public readonly IRequestClient<GetMoneyMqCmd> _moneyClient;
        public readonly IRequestClient<BuyInMqCmd> _buyInClient;
        private readonly IRequestClient<UserApplySitMqCmd> _sitClient;
        public MqManager(IBusControl bus,
            IRequestClient<GetAccountInfoMqCmd> accountClient,
            IRequestClient<GetMoneyMqCmd> moneyClient,
            IRequestClient<UserApplySitMqCmd> sitClient, 
            IRequestClient<BuyInMqCmd> buyInClient)
        {
            _bus = bus;
            _accountClient = accountClient;
            _moneyClient = moneyClient;
            _sitClient = sitClient;
            _buyInClient = buyInClient;
            //accountUrl = configuration.GetSection("Rabbitmq")["Account"];
            //moneyUrl = configuration.GetSection("Rabbitmq")["Money"];
        }

        public async Task<WrappedResponse<NullBody>> UserApplySit(long id, string roomId, string gameKey, long blind)
        {
            try
            {
                var response = await _sitClient.GetResponseExt<UserApplySitMqCmd, WrappedResponse<NullBody>>(
                    new UserApplySitMqCmd(id, roomId, gameKey, blind));
                return response.Message;
            }
            catch (Exception)
            {
                return null;
            }

        }

    

        public async Task<GetAccountInfoMqResponse> GetAccountInfo(long id)
        {
            try
            {
                var response = await _accountClient.GetResponseExt<GetAccountInfoMqCmd, WrappedResponse<GetAccountInfoMqResponse>>(new GetAccountInfoMqCmd(id));
                return response.Message.Body;
            }
            catch (Exception ex)
            {
                Log.Error($"user {id} GetMoneyInfo failed: {ex}");
                return null;
            }
            
        }

        public async Task<MoneyMqResponse> GetMoneyInfo(long id)
        {
            try
            {
                var response = await _moneyClient.GetResponseExt<GetMoneyMqCmd, WrappedResponse<MoneyMqResponse>>(new GetMoneyMqCmd(id));
                return response.Message.Body;
            }
            catch (Exception ex)
            {
                Log.Error($"user {id} GetMoneyInfo failed: {ex}");
                return null;
            }

        }

        public async Task<MoneyMqResponse> BuyIn(long id, long min, long max)
        {
            try
            {
                var response = await _buyInClient.GetResponseExt<BuyInMqCmd, WrappedResponse<MoneyMqResponse>>(
                    new BuyInMqCmd(id, min, max, AddReason.BuyIn));
                return response.Message.Body;
            }
            catch (Exception ex)
            {
                Log.Error($"user {id} BuyIn failed: {ex}");
                return null;
            }

        }
    }
}
