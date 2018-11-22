using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationLib
{
    [Serializable]
   public class Call
    {
        private GameC.Suites trump;
        private int bids;
        public Call()
        {

        }
        public Call(GameC.Suites t, int b)
        {
            this.trump = t;
            this.bids = b;
        }

        public int Bids { set { bids = value; } get { return bids; } }
        public GameC.Suites Trump { set { trump = value; } get { return trump; } }

        public bool SetCall(Call c) {
            if (c == null) return false;
            if (c.bids < 4)
                return false;
            if (c.bids < this.bids)
                return false;
            if (c.trump < this.trump&& c.bids<=this.bids)
                return false;
            
            if (c.bids == this.bids && this.trump == c.trump)
                return false;
            this.bids = c.bids;
            this.trump = c.trump;
            return true;

            
        }
        public override string ToString()
        {
            return bids + " " + trump.ToString();
        }
    }
}
