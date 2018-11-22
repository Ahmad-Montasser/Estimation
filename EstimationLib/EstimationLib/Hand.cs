using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationLib
{
    public class Hand
    {
        private Card[] cardArr;
        private GameC.Suites openSuit;
        private GameC.Suites trumpSuit;
        private int WinningIndex=0;
        public Hand(Card[] cardArr)
        {
            this.cardArr = cardArr;
        }
        public Hand(Card[] cardArr, GameC.Suites openSuit, GameC.Suites trumpSuit)
        {
            this.cardArr = cardArr;
            this.trumpSuit = trumpSuit;
            this.openSuit = openSuit;
        }
        public int ReturnWinningIndex()
        {
            ReturnWinningCard();
            for (int i = 0; i < cardArr.Length; i++)
            {
                Card maxCard = ReturnWinningCard();
                if (maxCard == cardArr[i])
                    WinningIndex = i;
            }
            return WinningIndex;
        }

        public Card ReturnWinningCard()
        {
            Card maxCard = cardArr[0];
            for (int i = 1; i < cardArr.Length; i++)
            {
                maxCard = Card.Compare(openSuit, trumpSuit, cardArr[i], maxCard);
            }
            return maxCard;
        }
    }

}

