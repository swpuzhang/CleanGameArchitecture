using Reward.Domain.Entitys;
using Reward.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reward.Domain.Manager
{
    public static class RewardManager
    {
        public static void LoadConfig(IGameActivityConfigRepository game,
            IRegisterRewardConfigRepository register,
            ILoginRewardConfigRepository login,
            IBankruptcyConfigRepository bankrupt,
            IInviteRewardConfigRepository invite)
        {
            GameActConf = new AllGameActivityConfig() { AllGameConfigs = game.LoadMultiConfig().ToList() };
            RegisterRewardConf = register.LoadConfig();
            LoginRewardConf = login.LoadConfig();
            BankruptcyConf = bankrupt.LoadConfig();
            InviteRewardConf = invite.LoadConfig();
        }
        public static AllGameActivityConfig GameActConf { get; private set; }
        public static RegisterRewardConfig RegisterRewardConf { get; private set; }
        public static LoginRewardConfig LoginRewardConf { get; private set; }
        public static BankruptcyConfig BankruptcyConf { get; private set; }

        public static InviteRewardConfig InviteRewardConf { get; private set; }

    }
}
