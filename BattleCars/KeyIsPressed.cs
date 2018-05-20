using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BattleCars
{
    public static class KeyIsPressed
    {
        public static bool P1_UP = false;
        public static bool P1_DOWN = false;
        public static bool P1_LEFT = false;
        public static bool P1_RIGHT = false;

        public static bool P2_UP = false;
        public static bool P2_DOWN = false;
        public static bool P2_LEFT = false;
        public static bool P2_RIGHT = false;

        public static void SetTrue(Key key)
        {
            switch (key)
            {

                //Player 1 keys
                case Key.W:
                    P1_UP = true;
                    break;
                case Key.A:
                    P1_LEFT = true;
                    break;
                case Key.S:
                    P1_DOWN = true;
                    break;
                case Key.D:
                    P1_RIGHT = true;
                    break;

                //Player 2 keys
                case Key.Up:
                    P2_UP = true;
                    break;
                case Key.Down:
                    P2_DOWN = true;
                    break;
                case Key.Left:
                    P2_LEFT = true;
                    break;
                case Key.Right:
                    P2_RIGHT = true;
                    break;
            }
        }

        public static void SetFalse(Key key)
        {
            switch (key)
            {
                //Player 1
                case Key.W:
                    P1_UP = false;
                    break;
                case Key.A:
                    P1_LEFT = false;
                    break;
                case Key.S:
                    P1_DOWN = false;
                    break;
                case Key.D:
                    P1_RIGHT = false;
                    break;

                //Player 2
                case Key.Up:
                    P2_UP = false;
                    break;
                case Key.Down:
                    P2_DOWN = false;
                    break;
                case Key.Left:
                    P2_LEFT = false;
                    break;
                case Key.Right:
                    P2_RIGHT = false;
                    break;
            }
        }
    }
}
