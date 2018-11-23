using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstimationLib;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Threading;

namespace EstimationClient
{
    class Program
    {
        static GameForm gf;
        static int P;
        static void Main(string[] args)
        {
            ConnectForm connectForm = new ConnectForm();
            Application.Run(connectForm);
            //TcpClient client = new TcpClient("127.0.0.1", 8000);
            TcpClient client = connectForm.Client;
            string name = connectForm.PlayerName;

            NetworkStream ns = client.GetStream();
            BinaryFormatter bf = new BinaryFormatter();
            

            bf.Serialize(ns, name);

            for (int i = 0; i < 18; i++)
            {
                gf = new GameForm(ns);
                Player p;
                p = (Player)bf.Deserialize(ns);
                gf.Player = p;
                gf.CardArr = p.PlayerCards;
                
                Application.EnableVisualStyles();
                Application.Run(gf);
            }


        }

        
    }
}
