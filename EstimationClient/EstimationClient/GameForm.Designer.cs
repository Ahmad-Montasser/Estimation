using System.Drawing;
using EstimationLib;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace EstimationClient
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Card[] handArr=new Card[4];
        public Card[] HandArr { set { handArr = value; } }
        private int indexTempCardArr = 0;
        public int IndexTempCardArr { set { indexTempCardArr = value; } }
        private CallForm callForm;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.SuspendLayout();
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(3168, 1712);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.ResumeLayout(false);
            handArr = new Card[4];
            this.MouseClick += GameForm_MouseClick;
            Invalidate();

        }
        private void GameForm_MouseClick(object sender, MouseEventArgs e)
        {

            if (!player.MyTurn)
                return;
            for (int i = 1; i <= 13; i++)
            {
                if (e.Y > GameC.Height - GameC.Height / 5 && e.X < (GameC.Width / 13 * (i)))
                {
                    Card selectedCard = player.PlayerCards[i - 1];
                    GetOpenSuite();
                    if (selectedCard == null)
                        return;

                    if (!player.CouldPlay(openSuite, selectedCard))
                        return;                    
                    Thread t = new Thread(SendCard);
                    t.Start(i);
                    //t.Join();
                    this.Invalidate();

                    break;
                }
            }

        }

        private void GetOpenSuite()
        {
            if (handArr[0] == null || handArr[3] != null)
                openSuite = GameC.Suites.Null;
            else
                openSuite = handArr[0].Suite;
        }
        #endregion

        private void SendCard(object i)
        {
            //handArr[indexTempCardArr % 4] = player.PlayCard((int)i);
            Card playedCard = player.PlayCard((int)i);
            BinaryFormatter bf = new BinaryFormatter();
            //bf.Serialize(ns, handArr[indexTempCardArr % 4]);
            bf.Serialize(ns, playedCard);
            ns.Flush();
            player.MyTurn = false;
        }
    }
}