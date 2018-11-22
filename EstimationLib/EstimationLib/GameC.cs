using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EstimationLib
{
    public class GameC
    {
        public enum CardSource { undetermined, down, right, up, left };
        public enum Suites { clubs, diamonds, hearts, Spades, Null }
        private static Dictionary<string, string> SuiteShape = new Dictionary<string, string>()
        {
            {"Spades", "♠" },
            {"hearts", "♥" },
            {"diamonds", "♦" },
            {"clubs", "♣" },
            {"♠" , "♠"},
            {"♥", "♥" },
            {"♦", "♦" },
            {"♣", "♣" }
        };
        public static int Width = Screen.PrimaryScreen.Bounds.Width;
        public static int Height = Screen.PrimaryScreen.Bounds.Height;
        public static int CardWidth = Width / 13;
        public static int CardHeight = Height / 6;

        public static string GetSuiteShape(string suite)
        {
            if (!SuiteShape.ContainsKey(suite))
                return suite;
            return SuiteShape[suite];
        }
    }
}
