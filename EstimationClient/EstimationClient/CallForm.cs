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
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Threading;

namespace EstimationClient
{
    public partial class CallForm : Form
    {
        Label[] lblArr = new Label[21];
        private Call call = new Call();        
        private String stringSuite = "";
        private String stringBids = "";
        private string title;
        private int lblNumber;
        private BinaryFormatter bf = new BinaryFormatter();
        private NetworkStream ns;
        private FlowLayoutPanel panel = new FlowLayoutPanel();

        public CallForm(NetworkStream ns, string title)
        {
            

            this.ns = ns;
            this.title = title;
            InitializeComponent();
            Text = title;
            Size = new Size(GameC.Width / 2, GameC.Height / 2);
            panel.Size = this.Size;
            Controls.Add(panel);
            AddLabels();
            AddLabelsText();


            if (!title.Equals("Place your call", StringComparison.CurrentCultureIgnoreCase))
                RemoveListeners();

        }

        private void RemoveListeners()
        {
            for (int i = 14; i < 18; i++)
            {
                lblArr[i].Click -= CallForm_Click;
            }
            lblArr[20].Click -= CallForm_Click;
            stringSuite = Text.Split(' ')[1];
            lblArr[18].Text = "Current Call:  " + GameC.GetSuiteShape(stringSuite);
        }        

        private void AddLabelsText()
        {
            for (int i = 0; i <= 13; i++)
            {
                lblArr[i].Text = i + "";
            }
            lblArr[17].Text = "♠";
            lblArr[16].Text = "♥";
            lblArr[15].Text = "♦";
            lblArr[14].Text = "♣";
            lblArr[18].Text = "Current Call: ";
            lblArr[19].Text = "Call";
            lblArr[20].Text = "Pass";
        }

        private void AddLabels()
        {
            for (int i = 0; i < 21; i++)
            {
                lblArr[i] = new Label();
                lblArr[i].Size = new Size(GameC.Width / 15, GameC.Height / 6);
                panel.Controls.Add(lblArr[i]);
                lblArr[i].TextAlign = ContentAlignment.MiddleCenter;
                if (i == 18)
                    continue;
                lblArr[i].Click += CallForm_Click;
            }
        }
    
        private void CallForm_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 14; i++)
            {
                if ((Label)sender == lblArr[i]) {
                    stringBids = lblArr[i].Text;

                    if (!title.Equals("Place your call", StringComparison.CurrentCultureIgnoreCase))
                        if (Convert.ToInt32(stringBids) > Convert.ToInt32(title.Split(' ')[0]))
                            return; // to unable the user to make bids more than the call bids

                    if (stringSuite != null)
                        stringSuite = GameC.GetSuiteShape(stringSuite);
                    lblArr[18].Text = "Current Call: " + stringBids + stringSuite ;
                    return;
                }
            }
            for (int i = 14; i < 18; i++)
            {
                if ((Label)sender == lblArr[i])
                {
                    stringSuite =GameC.GetSuiteShape(lblArr[i].Text);                    

                    lblArr[18].Text = "Current Call: " + stringBids + stringSuite;
                    lblNumber = i;
                    return;
                }
            }
            if ((Label)sender == lblArr[19])
            {
                if (!title.Equals("Place your call", StringComparison.CurrentCultureIgnoreCase))
                {
                    SendBids(Convert.ToInt32(stringBids));
                    Close();
                    return;
                }

                GameC.Suites suite = (GameC.Suites)lblNumber - 14;
                int bids = Convert.ToInt32(stringBids);
                call = new Call(suite, bids);
                SendCall(call);
                return;
            }

            if ((Label)sender == lblArr[20]) SendCall(new Call());
                
            


        }

        private void SendCall(Call call)
        {
            bf.Serialize(ns, call);
            ns.Flush();
            this.Close();
        }

        private void SendBids(int bids)
        {
            bf.Serialize(ns, bids);
            ns.Flush();
            GameForm.requiredHands = bids.ToString();
        }
        
    }
}
