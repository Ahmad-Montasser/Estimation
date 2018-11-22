using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace EstimationLib
{ [Serializable]
    public class TcpClient2:TcpClient
    {
        public TcpClient2():base()
        {
            
        }
        public TcpClient2(String s,int t):base(s,t)
        {

        }
    }
}
