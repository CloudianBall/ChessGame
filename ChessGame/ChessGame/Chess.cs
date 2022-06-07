using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ChessGame
{    
    public abstract class Chess
    {
        public ButtonXY PB;
        public chess_type type;
        public player_type side;

        public Chess()
        {
            side = player_type.blank;
            type = chess_type.blank;
        }
        public abstract bool Move_judge(object sender, int X, int Y, Chess[][] checkerboard);
        public abstract void Put_picture();

        public void Bg_toRed()
        {
            this.PB.BackColor = Color.Red;
        }
        public void Bg_toblank()
        {
            this.PB.BackColor = Color.Transparent;
        }

        public void ToBlank(ref Chess a)
        {
            this.Bg_toblank();
            this.PB.Text = "";
            a = new Chess_blank(this.PB);
        }
        public void ToBig(ref Chess a)
        {
            this.Bg_toblank();
            this.PB.Text = "L";
            a = new Chess_piece(chess_type.big, player_type.great,this.PB);
        }
        public void ToSmall(ref Chess a)
        {
            this.Bg_toblank();
            this.PB.Text = "S";
            a = new Chess_piece(chess_type.small, player_type.little,this.PB);
        }
    }
}
