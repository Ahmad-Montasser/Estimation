using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationLib
{
    public class Round
    {

        private Player[] players;       
        private int sa3ayda = 1;
        private GameC.Suites trump;

        public Player[] Players { get { return players; } }
        public int Sa3ayda { set { Sa3ayda = value; } get { return sa3ayda; } }
        public Round(Player[] players, GameC.Suites trump)
        {
            this.players = players;
            this.trump = trump;
            //for (int i = 0; i < 4; i++)
            //{
            //    players[i].IsCall = true;
            //}
        }
        private void SetMade() {
            for (int i = 0; i < 4; i++)
            {
                if (players[i].RequiredHands == players[i].CollectedHands)
                    players[i].Made = true;
                else
                    players[i].Made = false;
            
            }

        }
        public void CalculateScore()
        {
            SetMade();
            foreach (Player player in players)
            {
                if (player.Made)
                {
                    player.Score += 10 * sa3ayda;
                    player.Score += player.RequiredHands * sa3ayda;
                    if (player.IsCall)
                        player.Score += 10 * sa3ayda;
                    if (player.Risk)
                        player.Score += 10 * sa3ayda;
                    if (player.DoubleRisk)
                        player.Score += 20 * sa3ayda;
                }
                else
                {
                    player.Score -= player.RequiredHands * sa3ayda;
                    if (player.IsCall)
                        player.Score -= 10 * sa3ayda;
                    if (player.Risk)
                        player.Score -= 10 * sa3ayda;
                    if (player.DoubleRisk)
                        player.Score -= 20 * sa3ayda;

                }

            }
        }

        public void PlayHand(Card[] PlayedCards)
        {
                GameC.Suites OpenSuite=(GameC.Suites)5;
                for (int j = 0; j < players.Length; j++)
                {
                    players[j].ListCards();
                    Console.WriteLine("{0} select a card.", players[j].Name);
                    int position = Convert.ToInt32(Console.ReadLine());
                    Card played = players[j].PlayCard(position);
                    if (j == 0)
                        OpenSuite = played.Suite;
                    PlayedCards[j] = played;
                }
                Hand hand = new Hand(PlayedCards, OpenSuite, trump);
                int winner = hand.ReturnWinningIndex();
                players[winner].CollectedHands++;
                Console.WriteLine("{0} won the round.", players[winner].Name);
                UpdatePlayers(winner);

            
        }

        public void UpdatePlayers(int winner)
        {
            Player[] players1 = new Player[]{
                    players[winner],
                    players[winner].Next,
                    players[winner].Next.Next,
                    players[winner].Next.Next.Next
                };
            Array.Copy(players1, players, players.Length);
        }

        public void ArrangePlayers()
        {
            for (int i = 0; i < 4; i++)
            {
                if (players[i].IsCall)
                {
                    UpdatePlayers(i);
                }
            }
        }
    }   
}

