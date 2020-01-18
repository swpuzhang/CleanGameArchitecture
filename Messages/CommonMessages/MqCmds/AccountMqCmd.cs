using Commons.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonMessages.MqCmds
{
    public class GameInfoMq
    {

        public GameInfoMq()
        {
        }


        public GameInfoMq(int gameTimes, int winTimes, long maxWinCoins)
        {
            GameTimes = gameTimes;
            WinTimes = winTimes;
            MaxWinCoins = maxWinCoins;
        }

        public int GameTimes { get; set; }
        public int WinTimes { get; set; }
        public long MaxWinCoins { get; set; }
    }

    public class LevelInfoMq
    {

        public LevelInfoMq()
        {
        }

        public LevelInfoMq(int curLevel, int curExp, int needExp)
        {
            CurLevel = curLevel;
            CurExp = curExp;
            NeedExp = needExp;
        }

        public int CurLevel { get; set; }

        public int CurExp { get; set; }

        public int NeedExp { get; set; }
    }

    public class GetAccountInfoMqCmd
    {
        public GetAccountInfoMqCmd(long id)
        {
            Id = id;
        }
        public long Id { get; private set; }
    }

    public class GetAccountInfoMqResponse
    {
        public GetAccountInfoMqResponse(long id, string platformAccount,
            string userName, int sex, string headUrl,
            GameInfoMq gameInfo, LevelInfoMq levelInfo)
        {
            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            GameInfo = gameInfo;
            LevelInfo = levelInfo;
        }

        public Int64 Id { get; private set; }
        public string PlatformAccount { get; private set; }
        public string UserName { get; private set; }
        public int Sex { get; set; }
        public string HeadUrl { get; private set; }

        public GameInfoMq GameInfo { get; private set; }

        public LevelInfoMq LevelInfo { get; private set; }

    }

    public class GetAccountBaseInfoMqCmd
    {
        public GetAccountBaseInfoMqCmd(long id)
        {
            Id = id;
        }
        public long Id { get; private set; }
    }

    public class GetAccountBaseInfoMqResponse
    {
        [Flags]
        public enum SomeFlags : long
        {
            None = 0,
            RegisterReward = 0x1
        }
        public long Id { get; private set; }
        public string PlatformAccount { get; private set; }

        public string UserName { get; private set; }

        public int Sex { get; private set; }

        public string HeadUrl { get; private set; }

        public AccountType Type { get; set; }
        public DateTime RegisterDate { get; private set; }
        public SomeFlags Flags { get; private set; }

        public GetAccountBaseInfoMqResponse()
        {

        }
        public GetAccountBaseInfoMqResponse(long id, string platformAccount, string userName,
            int sex, string headUrl, AccountType type, DateTime registerDate)
        {

            Id = id;
            PlatformAccount = platformAccount;
            UserName = userName;
            Sex = sex;
            HeadUrl = headUrl;
            Type = type;
            RegisterDate = registerDate;
        }
    }

    public class GetIdByPlatformMqCmd
    {
        public GetIdByPlatformMqCmd(string platformAccount, int type)
        {
            PlatformAccount = platformAccount;
            Type = type;
        }

        public string PlatformAccount { get; private set; }
        public int Type { get; private set; }
    }

    public class GetIdByPlatformMqResponse
    {
        public GetIdByPlatformMqResponse(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }

}
