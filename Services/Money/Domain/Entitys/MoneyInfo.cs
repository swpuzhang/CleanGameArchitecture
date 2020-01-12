using Commons.Enums;
using Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Money.Domain.Entitys
{
    public class MoneyInfo : UserEntity
    {
        public static string ClassName = "MoneyInfo";
        public MoneyInfo()
        {
        }

        public MoneyInfo(long id, long curCoins, long curDiamonds, long maxCoins, long maxDiamonds, long carry)
        {
            Id = id;
            CurCoins = curCoins;
            CurDiamonds = curDiamonds;
            MaxCoins = maxCoins;
            MaxDiamonds = maxDiamonds;
            Carry = carry;
        }

        public void AddCoins(long coins)
        {
            CurCoins += coins;
        }

        public void AddCarry(long coins)
        {
            Carry += coins;
        }

        public void UpdateMaxCoins()
        {
            if (CurCoins + Carry > MaxCoins)
            {
                MaxCoins = CurCoins + Carry;
            }
        }

        public long CurCoins { get; private set; }
        public long CurDiamonds { get; private set; }
        public long MaxCoins { get; private set; }
        public long MaxDiamonds { get; private set; }
        public long Carry { get; private set; }
    }
}
