using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerEstimation
{
    public partial class ConnectForm : Form
    {
        private string ip;
        private TcpListener listener;
     
        public string IP { get { return ip; } }
        public TcpListener Listener { get { return listener; } }
        public ConnectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ip = txtIP.Text;
            if (Connect())
                Close();
            else
                MessageBox.Show("You entered a wrong IP");
        }

        private bool Connect()
        {
            try
            {                
               IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 8000);
                listener = new TcpListener(endPoint);
                listener.Start();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
