using Commons.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Domain.Entitys
{


    public class LevelInfo : UserEntity
    {
        public static string ClassName = "LevelInfo";
        public LevelInfo()
        {
        }

        public LevelInfo(long id, int curLevel, int curExp, int needExp)
        {
            Id = id;
            CurLevel = curLevel;
            CurExp = curExp;
            NeedExp = needExp;
        }

        public int CurLevel { get; set; }

        public int CurExp { get; set; }

        public int NeedExp { get; set; }
    }
}
