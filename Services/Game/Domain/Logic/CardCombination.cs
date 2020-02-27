using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CommonModels.GameModels;

namespace Game.Domain.Logic
{
    public enum CardCombinationType
    {
        Normal = 0,
        Flush = 1,
        Straight = 2,
        Sangong = 3,
        StraightFlush = 4,
        ThreeKind 
    }


     
    public class CardCombination : IComparable<CardCombination>
    {
        public CardCombinationType ComType { get; private set; }
        public List<PokerCard> Cards { get; private set; }

        public int Point { get; private set; }

        public CardCombination()
        {
            
        }

        public CardCombination(CardCombinationType comType, List<PokerCard> cards, int point)
        {
            ComType = comType;
            Cards = cards;
            Point = point;
        }

        public void Reset()
        {
            Cards = null;
            Point = 0;
            ComType = CardCombinationType.Normal;
        }

        public void SetCards(List<PokerCard> cards)
        {
            Cards = cards;
            int point = cards.Sum(x => x.Point);
            Point = point % 10;
            CaculateCombination();
        }

        public int CompareTo(CardCombination other)
        {
            int comType = (int)ComType;
            int ret = comType.CompareTo((int)other.ComType);
            if (ret != 0)
            {
                return ret;
            }
            if (ComType == CardCombinationType.ThreeKind)
            {
                return CompareThreeKind(other.Cards);
            }
            if (ComType == CardCombinationType.Flush || ComType == CardCombinationType.StraightFlush)
            {
                return CompareFlush(other.Cards);
            }
            if (ComType == CardCombinationType.Sangong || ComType == CardCombinationType.Straight)
            {
                return Cards[2].CompareTo(other.Cards[2]);
            }

            return CompareNormal(other);
        }

        public int CompareThreeKind(List<PokerCard> cards)
        {
            if (Cards[0].Point == (int)CardPoint.P3)
            {
                return 1;
            }
            if (cards[0].Point == (int)CardPoint.P3)
            {
                return -1;
            }

            return Cards[0].Point.CompareTo(cards[0].Point);
        }

        public int CompareFlush(List<PokerCard> cards)
        {
            return Cards[2].CompareFlush(cards[2]);
        }

        public int CompareNormal(CardCombination other)
        {
            int ret = Point.CompareTo(other.Point);
            if (ret == 0)
            {
                return Cards[2].CompareTo(other.Cards[2]);
            }
            return ret;
        }
       

       
        public void CaculateCombination()
        {
            Cards.Sort((l, r) =>
            {
                int ret = l.Point.CompareTo(r.Point);
                if (ret == 0)
                {
                    return l.Color.CompareTo(r.Color);
                }
                return ret;
            });
            if (Cards.Count != 3)
            {
                ComType = CardCombinationType.Normal;
                return;
            }
           
            if (IsThreeKind())
            {
                ComType = CardCombinationType.ThreeKind;
                return;
            }
            if (IsStraightFlush())
            {
                ComType = CardCombinationType.StraightFlush;
                return;
            }
            if (IsSangong())
            {
                ComType = CardCombinationType.StraightFlush;
                return;
            }
            if (IsStraight())
            {
                ComType = CardCombinationType.StraightFlush;
                return;
            }
            if (IsFlush())
            {
                ComType = CardCombinationType.StraightFlush;
                return;
            }
            ComType = CardCombinationType.Normal;
        }

        public  bool IsThreeKind()
        {
            
            return Cards[0].Point == Cards[1].Point && Cards[1].Point == Cards[2].Point;
        }

        public  bool IsStraightFlush()
        {
            return Cards[0].Color == Cards[1].Color && Cards[1].Color == Cards[2].Color &&
                 Cards[0].Point == Cards[1].Point - 1 && Cards[1].Point == Cards[2].Point - 1;
        }

        public bool IsSangong()
        {
           
            foreach (var card in Cards)
            {
                if (card.Point != (int)CardPoint.J &&
                    card.Point != (int)CardPoint.Q &&
                    card.Point != (int)CardPoint.K)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsStraight()
        {
            return Cards[0].Point == Cards[1].Point - 1 && Cards[1].Point == Cards[2].Point - 1;
        }

        public bool IsFlush()
        {
            return Cards[0].Color == Cards[1].Color && Cards[1].Color == Cards[2].Color;
        }

        
    }
    
}
