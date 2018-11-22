using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EstimationClient
{
    public partial class ConnectForm : Form
    {
        private string serverIP;
        private string name;
        private TcpClient client;
        public string PlayerName { get { return name; } }
        public TcpClient Client { get { return client; } }
        public ConnectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serverIP = txtIP.Text;
            name = txtName.Text;
            if (Connect())
                Close();
            else
                MessageBox.Show("You entered invalid IP address");
        }

        private bool Connect()
        {
            try
            {
                client = new TcpClient(serverIP, 8000);
            } catch (Exception e)
            {
                return false;
            }
            return true;
        }
        
    }
}
