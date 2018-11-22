using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EstimationLib;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ServerEstimation
{
    class Program
    {
        private static Player[] players;
        private static TcpClient[] clients;
        private static BinaryFormatter bf = new BinaryFormatter();
        private static NetworkStream ns;
        static Call call = new Call();
        
        static void Main(string[] args)
        {
            ConnectForm connectForm = new ConnectForm();
            Application.Run(connectForm);
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            
            players = new Player[4];
            //TcpListener listener = new TcpListener(endPoint);
            TcpListener listener = connectForm.Listener;
            clients = new TcpClient[4];
            List<Card> playerCards = new List<Card>(13);
            ConnectClients(listener);
            InitializeNames();

            for (int l = 0; l < 18; l++)
            {
                #region Intialize player 
                Deck deck = new Deck();
                for (int i = 0; i < players.Length; i++)
                {
                    playerCards = deck.GetPlayerCards();
                    int clientIndex = players[i].ClientIndex;
                    ns = clients[clientIndex].GetStream();
                    players[i].PlayerCards = playerCards;
                    bf.Serialize(ns, players[i]);
                    ns.Flush();
                }
                #endregion
                #region call logic
                int caller = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    int clientIndex = players[i].ClientIndex;
                    NetworkStream ns = clients[clientIndex].GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    Call tempCall = (Call)bf.Deserialize(ns);

                    if (call.SetCall(tempCall))
                    {
                        players[caller].IsCall = false;
                        caller = i;
                        players[caller].IsCall = true;
                    }
                    Console.WriteLine(call);

                }
                SendCallToPlayers();

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].IsCall)
                    {
                        players[i].RequiredHands = call.Bids;
                        continue;
                    }

                    int clientIndex = players[i].ClientIndex;
                    NetworkStream ns = clients[clientIndex].GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    players[i].RequiredHands = (int)bf.Deserialize(ns);

                }
                #endregion

                #region round logic
                Round round = new Round(players, call.Trump);
                round.ArrangePlayers();
                for (int j = 0; j < 13; j++)
                {
                    Card[] playedCards = new Card[4];
                    GameC.Suites openSuite = GameC.Suites.Null;
                    for (int i = 0; i < players.Length; i++)
                    {
                        int clientIndex = players[i].ClientIndex;
                        ns = clients[clientIndex].GetStream();
                        SendTurns(i);
                        Card tempCard = (Card)bf.Deserialize(ns);
                        if (i == 0)
                            openSuite = tempCard.Suite;
                        SendCardToPlayers(tempCard, i);
                        Console.WriteLine(tempCard.ToString());
                        playedCards[i] = tempCard;
                    }
                    Hand hand = new Hand(playedCards, openSuite, call.Trump);
                    int handWinner = hand.ReturnWinningIndex();
                    players[handWinner].CollectedHands++;
                    //SendWinnerToPlayers(handWinner); // send the client index of the winner player to the players
                    // to update his collected hands
                    

                    round.UpdatePlayers(handWinner);
                }
                round.CalculateScore();
                
                #endregion
            }
        }


        private static void SendTurns(int index)
        {
            for (int i = 0; i < players.Length; i++)
            {
                int clientIndex = players[i].ClientIndex;
                NetworkStream ns = clients[clientIndex].GetStream();
                if (index == i)
                    bf.Serialize(ns, true);
                else
                    bf.Serialize(ns, false);
                ns.Flush();
            }
        }
        private static void InitializeNames()
        {
            for (int i = 0; i < players.Length; i++)
            {
                int clientIndex = players[i].ClientIndex;
                NetworkStream ns = clients[clientIndex].GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                players[i].Name = (string)bf.Deserialize(ns);
            }

        }
        private static void ConnectClients(TcpListener listener)
        {
            for (int i = 0; i < players.Length; i++)
            {
                clients[i] = listener.AcceptTcpClient();
                players[i] = new Player(i);
                Console.WriteLine("Client Connected");
            }
            players[0].Next = players[1];
            players[1].Next = players[2];
            players[2].Next = players[3];
            players[3].Next = players[0];

        }
        private static void SendCardToPlayers(Card tempCard, int playerPosition)
        {
            for (int i = 0; i < players.Length; i++)
            {
                int clientIndex = players[i].ClientIndex;
                ns = clients[clientIndex].GetStream();
                GameC.CardSource cardSource = players[i].DetermineSender(players[playerPosition]);
                tempCard.CardSender = cardSource;
                bf.Serialize(ns, tempCard);
                //bf.Serialize(ns, cardSource);
                ns.Flush();

            }
        }
        private static void SendCallToPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                int clientIndex = players[i].ClientIndex;
                ns = clients[clientIndex].GetStream();
                bf.Serialize(ns, call);
                if (players[i].IsCall)
                    bf.Serialize(ns, "pass");
                else
                    bf.Serialize(ns, call.ToString());

                ns.Flush();

            }
        }

        private static void SendWinnerToPlayers(int winnerClientIndex)
        {           
            for (int i = 0; i < players.Length; i++)
            {
                int clientIndex = players[i].ClientIndex;
                ns = clients[clientIndex].GetStream();
                
                bf.Serialize(ns, players[winnerClientIndex].ClientIndex);

                ns.Flush();

            }
        }
    }
}
