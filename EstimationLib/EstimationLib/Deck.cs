using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationLib
{
    public class Deck
    {
        private List<Card> cards = new List<Card>(52);
        private List<Card> playerCards;
        private List<Card> spades;
        private List<Card> hearts;
        private List<Card> diamonds;
        private List<Card> clubs;


        public Deck()
        {
            GenerateCards();
        }

        public void ListCards()
        {
            foreach (Card card in cards)
                Console.WriteLine(card);
        }

        private void GenerateCards()
        {
            for (int i = 0; i < 52; i++)
            {
                cards.Add(Card.GenerateCard(i));
            }
        }

        /// <summary>
        /// Returns a list of 13 cards selected randomly from the deck
        /// </summary>
        /// <returns>Cards that the user will hold</returns>
        public List<Card> GetPlayerCards()
        {
            playerCards = new List<Card>(13);
            clubs = new List<Card>();
            diamonds = new List<Card>();
            hearts = new List<Card>();
            spades = new List<Card>();


            Random random = new Random(); // used to shuffle the deck at the start

            for (int i = 0; i < 13; i++)
            {
                int cardPosition = random.Next(cards.Count); // generates a random position
                Card selectedCard = cards[cardPosition];
                AddCardToPlace(selectedCard);
                //playerCards.Add(cards[cardPosition]); // adds the card at the specified position to the user's cards
                cards.RemoveAt(cardPosition); // remove the card from the deck
            }
            SortCards(playerCards);
            return playerCards;
        }

        private void AddCardToPlace(Card selectedCard)
        {
            if (selectedCard.Suite==GameC.Suites.Spades)
                spades.Add(selectedCard);
            else if (selectedCard.Suite == GameC.Suites.hearts)
                hearts.Add(selectedCard);
            else if (selectedCard.Suite == GameC.Suites.diamonds)
                diamonds.Add(selectedCard);
            else
                clubs.Add(selectedCard);
        }

        private void SortCards(List<Card> playerCards)
        {
            spades.Sort();
            hearts.Sort();
            diamonds.Sort();
            clubs.Sort();
            playerCards.AddRange(spades);
            playerCards.AddRange(hearts);
            playerCards.AddRange(clubs);
            playerCards.AddRange(diamonds);
        }
    }
}
