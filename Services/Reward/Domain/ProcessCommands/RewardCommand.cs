using Commons.Buses.ProcessBus;
using System;
using System.Collections.Generic;
using System.Text;
using Commons.Models;
using Reward.Domain.Entitys;
using Reward.ViewModels;

namespace Reward.Domain.Commands
{
    public class GetRegisterRewardCommand : ProcessCommand<WrappedResponse<RewardInfoVm>>
    {
        public long Id  { get; private set; }
        public GetRegisterRewardCommand(long id)
        {
            Id = id;
        }
    }

    public class QueryRegisterRewardCommand : ProcessCommand<WrappedResponse<RegisterRewardVm>>
    {
        public long Id { get; private set; }
        public QueryRegisterRewardCommand(long id)
        {
            Id = id;
        }
    }

    public class QueryLoginRewardCommand : ProcessCommand<WrappedResponse<LoginRewardVm>>
    {
        public long Id { get; private set; }
        public QueryLoginRewardCommand(long id)
        {
            Id = id;
        }
    }

    public class GetLoginRewardCommand : ProcessCommand<WrappedResponse<RewardInfoVm>>
    {
        public long Id { get; private set; }
        public GetLoginRewardCommand(long id)
        {
            Id = id;
        }
    }

    public class QueryBankruptcyCommand : ProcessCommand<WrappedResponse<BankruptcyInfoVm>>
    {
        public long Id { get; private set; }
        public QueryBankruptcyCommand(long id)
        {
            Id = id;
        }
    }

    public class GetBankruptcyRewardCommand : ProcessCommand<WrappedResponse<RewardInfoVm>>
    {
        public long Id { get; private set; }
        public GetBankruptcyRewardCommand(long id)
        {
            Id = id;
        }
    }
}
