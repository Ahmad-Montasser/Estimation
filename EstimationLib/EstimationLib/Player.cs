using System;
using System.Collections.Generic;

namespace EstimationLib
{
    [Serializable]
    public class Player
    {
        private GameC.CardSource cardSender;
        private string name;
        private List<Card> playerCards;
        private bool myTurn = false;
        private bool isCall = false;
        private bool avoidDiamonds;
        private bool avoidHearts;
        private bool avoidSpades;
        private bool avoidClubs;
        private string isInitialAvoid = "";
        private bool risk;
        private bool doubleRisk;
        private int score = 0;
        private bool made;
        private int requiredHands;
        private int collectedHands = 0;
        private Player next;
        private int clientIndex;

        public string IsInitialAvoid { get { return isInitialAvoid; } }
        public GameC.CardSource CardSender { get { return cardSender; } set { cardSender = value; } }
        public bool MyTurn { get { return myTurn; } set { myTurn = value; } }
        public int ClientIndex { get { return clientIndex; } }
        public Player Next { get { return next; } set { next = value; } }
        public int CollectedHands { get { return collectedHands; } set { collectedHands = value; } }
        public bool Made { get { return made; } set { made = value; } }
        public int Score { get { return score; } set { score = value; } }
        public int RequiredHands { get { return requiredHands; } set { requiredHands = value; } }
        public bool IsCall { get { return isCall; } set { isCall = value; } }
        public bool Risk { get { return risk; } set { risk = value; } }
        public bool DoubleRisk { get { return doubleRisk; } set { doubleRisk = value; } }
        public String Name { get { return name; } set { name = value; } }
        public List<Card> PlayerCards { get { return playerCards; }
            set
            {
                playerCards = value;
                DetermineAvoidness();
                if (avoidClubs || avoidDiamonds || avoidHearts || avoidSpades)
                    isInitialAvoid = "Avoid";
            }
        }

        

        public Player(int clientIndex)
        {
            this.clientIndex = clientIndex;
        }

        public void ListCards()
        {
            foreach (Card card in playerCards)
                Console.WriteLine(card);
        }

        /// <summary>
        /// Gets a position of a card in the user's hand and remove this card
        /// </summary>
        /// <param name="position"> position of card</param>
        public Card PlayCard(int position)
        {   position = position - 1; // to convert to zero based
            Card played = playerCards[position];
            playerCards[position] = null;
            return played;            
        }

        public GameC.CardSource DetermineSender(Player sender)
        {
            if (sender == this)
                return GameC.CardSource.down;
            else if (sender == next)
                return GameC.CardSource.right;
            else if (sender == next.next)
                return GameC.CardSource.up;
            else
                return GameC.CardSource.left;
        }

        public bool CouldPlay(GameC.Suites openSuite, Card cardToPlay)
        {
            DetermineAvoidness();

            if (openSuite == cardToPlay.Suite || openSuite == GameC.Suites.Null)
                return true;
            if (openSuite == GameC.Suites.Spades && avoidSpades)
                return true;
            if (openSuite == GameC.Suites.hearts && avoidHearts)
                return true;
            if (openSuite == GameC.Suites.diamonds && avoidDiamonds)
                return true;
            if (openSuite == GameC.Suites.clubs && avoidClubs)
                return true;

            return false;
        }

        private void DetermineAvoidness()
        {
            avoidSpades = true;
            avoidHearts = true;
            avoidDiamonds = true;
            avoidClubs = true;

            foreach (Card card in playerCards)
            {
                if (card == null)
                    continue;
                if (card.Suite == GameC.Suites.Spades)
                    avoidSpades = false;
                else if (card.Suite == GameC.Suites.hearts)
                    avoidHearts = false;
                else if (card.Suite == GameC.Suites.diamonds)
                    avoidDiamonds = false;
                else if (card.Suite == GameC.Suites.clubs)
                    avoidClubs = false;
            }
        }       
    }
}
