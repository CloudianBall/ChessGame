using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    public class Chess_piece : Chess
    {
        public Chess_piece(chess_type type, player_type player_)
        {
            base.type = type;
            base.side = player_;
        }
        public Chess_piece(chess_type type, player_type player_, ButtonXY button)
        {
            base.type = type;
            base.side = player_;
            this.PB = button;
        }

        /// <summary>
        /// Judge Piece Move
        /// </summary>
        /// <param name="sender">target button</param>
        /// <param name="X">selected button's X</param>
        /// <param name="Y">selected button's Y</param>
        /// <param name="checkerboard">total chesses array</param>
        /// <returns></returns>
        public override bool Move_judge(object sender, int X, int Y, Chess[][] checkerboard)
        {
            bool isMove = false;
            ButtonXY button = sender as ButtonXY;
            if (button.X == X && button.Y == Y)
                return false;
            if (button.X != X && button.Y != Y)
                return false;
            switch (checkerboard[X][Y].type)
            {
                case chess_type.big:
                    if(button.X == X)
                    {
                        if (Math.Abs(button.Y - Y) == 1 && checkerboard[button.X][button.Y].type == chess_type.blank)
                        {
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "L" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToBig(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                        }
                        else if (Math.Abs(button.Y - Y) == 2 && checkerboard[button.X][button.Y].type == chess_type.small)
                        {
                            if (checkerboard[X][(button.Y + Y) / 2].type != chess_type.blank)
                                break;
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "L" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "S" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToBig(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                            Form1.SmallPieceCount--;
                        }
                    }
                    else if(button.Y == Y)
                    {
                        if (Math.Abs(button.X - X) == 1 && checkerboard[button.X][button.Y].type == chess_type.blank)
                        {
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "L" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToBig(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                        }
                        else if (Math.Abs(button.X - X) == 2 && checkerboard[button.X][button.Y].type == chess_type.small)
                        {
                            if (checkerboard[(button.X + X) / 2][Y].type != chess_type.blank)
                                break;
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "L" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "S" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToBig(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                            Form1.SmallPieceCount--;
                        }
                    }
                    if(isMove)
                    {
                        Form1.LargePieces.Remove(Form1.LargePieces.Find(t => t.PB.X == X && t.PB.Y == Y));
                        Form1.LargePieces.Add(checkerboard[button.X][button.Y]);
                    }
                    break;
                case chess_type.small:
                    if (button.X == X)
                    {
                        if (Math.Abs(button.Y - Y) == 1 && checkerboard[button.X][button.Y].type == chess_type.blank)
                        {
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "S" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToSmall(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                        }
                    }
                    else if (button.Y == Y)
                    {
                        if (Math.Abs(button.X - X) == 1 && checkerboard[button.X][button.Y].type == chess_type.blank)
                        {
                            Form1.records.Add(
                                Tuple.Create<Chess, Chess>(
                                    new Chess_piece(checkerboard[X][Y].type, checkerboard[X][Y].side, new ButtonXY(X, Y) { Text = "S" }),
                                    new Chess_piece(checkerboard[button.X][button.Y].type, checkerboard[button.X][button.Y].side, new ButtonXY(button.X, button.Y) { Text = "" })
                                    ));
                            checkerboard[X][Y].ToBlank(ref checkerboard[X][Y]);
                            checkerboard[button.X][button.Y].ToSmall(ref checkerboard[button.X][button.Y]);
                            isMove = true;
                        }
                    }
                    break;
            }
            return isMove;
        }

        public override void Put_picture()
        {
            // Have no picture resources for now.
            return;
        }
    }
}
