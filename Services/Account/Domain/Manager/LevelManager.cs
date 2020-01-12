using Account.Domain.Entitys;
using Account.Infrastruct.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extenssions;
using System.Threading.Tasks;

namespace Account.Domain.Manager
{
    public static class LevelManager
    {
        private readonly static Dictionary<int, LevelConfig> _levelConfigs = new Dictionary<int, LevelConfig>();
        public static bool IsLoaded { get; private set; } = false;
        public static void LoadConfig(ILevelConfigRepository repository)
        {
            if (IsLoaded)
            {
                return;
            }
            var configs = repository.LoadMultiConfig();
            configs.ForEach(p => _levelConfigs.Add(p.Level, p));
            IsLoaded = true;
        }

        public static int GetNeedExp(int curLevel)
        {
            if (!IsLoaded)
            {
                throw new Exception("level config hasn't loaded");
            }
            var lastConfig = _levelConfigs[_levelConfigs.Keys.Last<int>()];
            if (curLevel >= lastConfig.NeedExp)
            {
                return lastConfig.NeedExp;
            }

            return _levelConfigs[curLevel + 1].NeedExp;
        }

        public static bool AddExp(LevelInfo info, int addExp)
        {
            if (!IsLoaded)
            {
                throw new Exception("level config hasn't loaded");
            }
            if (!_levelConfigs.TryGetValue(info.CurLevel + 1, out var config))
            {
                return false;
            }

            var lastConfig = _levelConfigs[_levelConfigs.Keys.Last<int>()];
            if (info.CurLevel > lastConfig.Level)
            {
                return false;
            }
            if (info.CurLevel == lastConfig.Level && info.CurExp == lastConfig.NeedExp)
            {
                return false;
            }
            info.CurExp += addExp;
            info.NeedExp = config.NeedExp;
            var remaindExp = info.CurExp - config.NeedExp;
            if (remaindExp < 0)
            {
                //如果是当前经验减成了负数
                if (info.CurExp < 0)
                {
                    info.CurExp = 0;
                }
                return true;
            }
            ++info.CurLevel;
            if (info.CurLevel > lastConfig.Level)
            {
                --info.CurLevel;
                info.CurExp = lastConfig.NeedExp;
                info.NeedExp = lastConfig.NeedExp;
                return true;
            }

            info.CurExp = remaindExp;
            info.NeedExp = config.NeedExp;
            return true;
        }
    }
}
