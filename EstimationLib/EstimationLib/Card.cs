using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationLib
{
    [Serializable]
    public class Card:IComparable<Card>
    {
        private int number; // the number of the card (id)
        private int value;
        
        private GameC.Suites suite;
        private GameC.CardSource cardSender = GameC.CardSource.undetermined;
        public GameC.CardSource CardSender { get { return cardSender; } set { cardSender = value; } }
        public int Value { get { return value; } }
        public GameC.Suites Suite { get { return suite; } }
        private Card(int number, int value, GameC.Suites suite)
        {
            this.number = number;
            this.value = value;
            this.suite = suite;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", suite, value);
        }

        /// <summary>
        /// Takes a number(id) and returns a card for that specific number
        /// </summary>
        /// <param name="counter">This will be the ID of the card</param>
        /// <returns>Generated Card object</returns>
        public static Card GenerateCard(int counter)
        {
            int value = (counter % 13) + 1; // gets the value from 1 to 13
            GameC.Suites suite;
            switch (counter / 13) // divide the deck to 4 suites
            {
                case 0:
                    suite = 0;
                    break;
                case 1:
                    suite = (GameC.Suites)1;
                    break;
                case 2:
                    suite = (GameC.Suites)2;
                    break;
                default:
                    suite = (GameC.Suites)3;
                    break;
            }
            return new Card(counter, value, suite);
        }

        public int GetNumber() { return number; }
        public void Draw(Graphics g, int i)
        {
            g.DrawImage(
                Image.FromFile(String.Format(@"Cards\card" + Convert.ToString(this.suite.ToString()) + Convert.ToString(this.value) + ".png")), 
                (GameC.CardWidth * i), 
                GameC.Height-GameC.Height/5, 
                GameC.CardWidth, 
                GameC.CardHeight
                );
        }
        public void Draw(Graphics g, int x,int y)
        {
            g.DrawImage(
                Image.FromFile(String.Format(@"Cards\card" + Convert.ToString(this.suite.ToString()) + Convert.ToString(this.value) + ".png")),
                x,
                y,
                GameC.CardWidth,
                GameC.CardHeight
                );
        }
        public void Draw(Graphics g)
        {
            if (cardSender == GameC.CardSource.left)
                Draw(g, GameC.CardWidth * 5, GameC.CardHeight * 2);
            
            else if (cardSender == GameC.CardSource.right)
                Draw(g, GameC.CardWidth * 7, GameC.CardHeight * 2);
          

            else if (cardSender == GameC.CardSource.down)
                Draw(g, GameC.CardWidth * 6, GameC.CardHeight * 3);
           
            else if (cardSender == GameC.CardSource.up)
                Draw(g, GameC.CardWidth * 6, GameC.CardHeight);
                
            
        }

        public int CompareTo(Card other)
        {
            return value.CompareTo(other.value);
        }

        public static Card Compare(GameC.Suites openSuite, GameC.Suites trump, Card c1, Card c2)
        {
            if (c1.suite == trump && c2.suite != trump)
                return c1;
            else if (c2.suite == trump && c1.suite != trump)
                return c2;
            else if (c2.suite == openSuite && c1.suite != openSuite)
                return c2;
            else if (c1.suite == openSuite && c2.suite != openSuite)
                return c1;
            else
                return (c1.value > c2.value) ? c1 : c2;

        }

    }
}
