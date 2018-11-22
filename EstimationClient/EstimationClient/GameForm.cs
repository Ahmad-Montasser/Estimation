using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EstimationLib;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace EstimationClient
{
    public partial class GameForm : Form
    {
        private String[] nameArr = new string[4];
        private Player[] players = new Player[4];
        private List<Card> cardArr;
        private Player player;
        private BinaryFormatter bf = new BinaryFormatter();
        private NetworkStream ns;
        private Call call;
        private GameC.Suites openSuite = GameC.Suites.Null;
        public static string requiredHands = null;

        public Player Player {
            set {
                player = value;
                SetPlayersArray();
                //SetPlayerInfo();
                Invalidate();
            }
        }

        private void SetPlayersArray()
        {
            players[0] = player;
            players[1] = player.Next;
            players[2] = player.Next.Next;
            players[3] = player.Next.Next.Next;

        }

        private void SetPlayerInfo()
        {
            for (int i = 0; i < players.Length; i++)
                nameArr[i] = string.Format("{0} {1}", players[i].Name, players[i].Score);
                    //+ "\n" + string.Format("{0}/{1} ", players[i].CollectedHands, players[i].RequiredHands) + 
                    //string.Format(" {0}", players[i].IsInitialAvoid);            
        }

        public List<Card> CardArr { set { cardArr = value; Invalidate(); } }
        private int tempHandArrindex = 0;
        private void GameForm_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(GameLoop);
            t.Start();
        }


        public GameForm(NetworkStream ns)
        {
            this.ns = ns;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Size = new Size(GameC.Width, GameC.Height);
            callForm = new CallForm(ns, "Place your call");
        }

        public void GameLoop()
        {
            Application.Run(callForm);
            call = (Call)bf.Deserialize(ns);
            string title = (string)bf.Deserialize(ns);
            if (!title.Equals("pass"))
            {
                callForm = new CallForm(ns, title);
                Application.Run(callForm);
            }
            this.Text = call.ToString();
            StartGame();

        }

        private void StartGame()
        {
            for (int i = 0; i < 52; i++)
            {

                bool turn = (bool)bf.Deserialize(ns);
                player.MyTurn = turn;
                Console.WriteLine(player.MyTurn);//For Debugging Reasons
                Card recievedCard = (Card)bf.Deserialize(ns);
                
                Console.WriteLine(recievedCard.ToString());
                Console.WriteLine(recievedCard.CardSender.ToString());
                if (tempHandArrindex % 4 == 0)
                {
                    //UpdateCollectedHandsForTheWinner();
                    handArr = new Card[4];
                    tempHandArrindex = 0;
                }
                AddCardToHand(recievedCard);
            }
            this.Close();
        }

        private void UpdateCollectedHandsForTheWinner()
        {
            int clientIndex = (int)bf.Deserialize(ns);
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].ClientIndex == clientIndex)
                    players[i].CollectedHands++;
            }
        }

        private void AddCardToHand(Card recievedCard)
        {
            handArr[tempHandArrindex] = recievedCard;
            tempHandArrindex++;
            Invalidate();
        }

        private void StartNewHand(Card recievedCard)
        {
            handArr = new Card[4];
            AddCardToHand(recievedCard);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw(e.Graphics);

        }
        private void Draw(Graphics g)
        {

            for (int i = 0; i < player.PlayerCards.Count; i++)
            {
                Card card = player.PlayerCards[i];
                if (card != null)
                    card.Draw(g, i);
            }

            for (int i = 0; i < handArr.Length; i++)
            {
                Card card = handArr[i];
                if (card == null)
                    continue;
                card.Draw(g);
            }
            SetPlayerInfo();
            g.DrawString(nameArr[0], new Font("Calibri", GameC.Width / 100), Brushes.FloralWhite, GameC.CardWidth * 6, GameC.CardHeight*9/2);
            g.DrawString(nameArr[1], new Font("Calibri", GameC.Width / 100), Brushes.FloralWhite, GameC.CardWidth * 12, GameC.Height/ 2);
            g.DrawString(nameArr[2], new Font("Calibri", GameC.Width / 100), Brushes.FloralWhite, GameC.CardWidth * 6, GameC.CardHeight /2);
            g.DrawString(nameArr[3], new Font("Calibri", GameC.Width / 100), Brushes.FloralWhite, GameC.CardWidth/2, GameC.Height / 2);
           
        }

    }
}
