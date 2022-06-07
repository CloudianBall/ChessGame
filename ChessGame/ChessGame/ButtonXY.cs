using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class ButtonXY : System.Windows.Forms.Button
    {
        public int X;
        public int Y;

        public ButtonXY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
