using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessGame.GameAI
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position()
        {
            X = 0;
            Y = 0;
        }
        public Position(int X,int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    public class VirtualChess
    {
        public chess_type type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public VirtualChess(chess_type type, int x, int y)
        {
            this.type = type;
            this.X = x;
            this.Y = y;
        }
    }

    public class Step : IComparable, IComparer<Step>
    {
        public chess_type type;
        public Position SourcePos;
        public Position TargetPos;
        public int Score;

        public Step()
        {
            type = chess_type.blank;
            SourcePos = new Position();
            TargetPos = new Position();
            Score = 0;
        }
        public Step(chess_type type, Position srcPos, Position tarPos)
        {
            this.type = type;
            this.SourcePos = srcPos;
            this.TargetPos = tarPos;
            Score = 0;
        }

        public int Compare([AllowNull] Step x, [AllowNull] Step y)
        {
            if (x.Score < y.Score)
                return 1;
            else if (x.Score == y.Score)
                return 0;
            else
                return -1;
        }

        public int CompareTo(object obj)
        {
            if (this.Score > ((Step)obj).Score)
                return -1;
            else if (this.Score == ((Step)obj).Score)
                return 0;
            else
                return 1;
        }
    }


    public class GameAI
    {
        /// <summary>
        /// A fake Infinity
        /// </summary>
        protected const int INFINITY = 100000;
        protected const int SMALL_VALUE = 20;
        protected const int SMALL_MOVE_VALUE = 5;
        protected const int BIG_MOVE_VALUE = 10;

        public Dictionary<int,Step> MoveLists;
        public Step BestStep;

        protected int x;
        public int X
        {
            get
            {
                return x;
            }
        }
        protected int y;
        public int Y
        {
            get
            {
                return y;
            }
        }
        protected chess_type winType;
        public chess_type WinType
        {
            get
            {
                return winType;
            }
        }
        protected int maxNPly;
        public int MaxNPly
        {
            get
            {
                return maxNPly;
            }
        }
        public player_type AiSide;
        public VirtualChess[][] board;
        public Dictionary<int, int> history_score;
        public int AlgorithmDepth;

        public GameAI(int x, int y, int maxNPly, player_type side)
        {
            this.x = x;
            this.y = y;
            this.maxNPly = maxNPly;
            this.AiSide = side;
            MoveLists = new Dictionary<int, Step>();
            InitBoard();
        }

        public void InitBoard()
        {
            board = new VirtualChess[this.X][];
            for (int i = 0; i < this.X; i++)
            {
                board[i] = new VirtualChess[this.Y];
                for (int j = 0; j < this.Y; j++)
                {
                    board[i][j] = new VirtualChess(Form1.Chesses[i][j].type, Form1.Chesses[i][j].PB.X, Form1.Chesses[i][j].PB.Y);
                }
            }
        }

        public void UpdateBoard(Tuple<Chess, Chess> lastMove)
        {
            Position srcPos = new Position(lastMove.Item1.PB.X, lastMove.Item1.PB.Y);
            Position tarPos = new Position(lastMove.Item2.PB.X, lastMove.Item2.PB.Y);
            Step lastStep = new Step(lastMove.Item1.type, srcPos, tarPos);
            MakeMove(lastStep);
        }

        private chess_type MakeMove(Step step)
        {
            chess_type tarType = board[step.TargetPos.X][step.TargetPos.Y].type;

            board[step.TargetPos.X][step.TargetPos.Y].type = board[step.SourcePos.X][step.SourcePos.Y].type;
            board[step.SourcePos.X][step.SourcePos.Y].type = chess_type.blank;

            return tarType;
        }

        private void UnMakeMove(Step step, chess_type recoverType)
        {
            board[step.SourcePos.X][step.SourcePos.Y].type = step.type;
            board[step.TargetPos.X][step.TargetPos.Y].type = recoverType;
        }
        /// <summary>
        /// Check how many large pieces are around a piece
        /// </summary>
        /// <param name="aX"></param>
        /// <param name="aY"></param>
        /// <returns></returns>
        private int CheckLargePiece(int aX,int aY)
        {
            int count = 0;

            for (int i = aX - 1; i <= aX + 1; i++)
            {
                if (i < 0 || i >= 5)
                    continue;
                for (int j = aY - 1; j <= aY + 1; j++)
                {
                    if (j < 0 || j >= 5)
                        continue;
                    if (i == aX && j == aY)
                        continue;
                    if (Math.Abs(i - aX) == 1 && Math.Abs(j - aY) == 1)
                        continue;
                    if (board[i][j].type == chess_type.big)
                        count++;
                }
            }

            return count;
        }
        private void SortMove(List<Step> steps)
        {
            for (int i = steps.Count - 1; i > 0; i--)
            {
                Step step = steps[i];
                switch(board[step.TargetPos.X][step.TargetPos.Y].type)
                {
                    case chess_type.blank:  // Give priority to the vacancy to which the larger piece can be moved
                        int count = CheckLargePiece(step.TargetPos.X, step.TargetPos.Y);
                        if(step.type == chess_type.big)
                            step.Score = -BIG_MOVE_VALUE * count * (1 << AlgorithmDepth) / 10;
                        else if(step.type == chess_type.small)
                            step.Score = BIG_MOVE_VALUE * count * (1 << AlgorithmDepth) / 10;
                        break;
                    case chess_type.small:  // Take the small pieces first
                        step.Score = SMALL_VALUE * (1 << AlgorithmDepth) / 10;
                        break;
                    default:
                        step.Score = 0;
                        break;
                }

                int from_chess = GetStepNum(step);
                if(history_score.ContainsKey(from_chess))
                {
                    step.Score += history_score[from_chess]; 
                }
            }

            steps.Sort();
        }

        public Boolean IsValidMove(VirtualChess[][] chesses, int nFromX, int nFromY, int nToX, int nToY)
        {
            chess_type nMoveChess, nTarget;

            if (nFromX == nToX && nFromY == nToY)
                return false;
            if (nFromX != nToX && nFromY != nToY)
                return false;
            if (nFromX < 0 || nFromX >= X)
                return false;
            if (nFromY < 0 || nFromY >= Y)
                return false;
            if (nToX < 0 || nToX >= X)
                return false;
            if (nToY < 0 || nToY >= Y)
                return false;
            nMoveChess = chesses[nFromX][nFromY].type;
            nTarget = chesses[nToX][nToY].type;
            if (nMoveChess == nTarget)
                return false;
            switch(nMoveChess)
            {
                case chess_type.big:
                    if(nToX == nFromX)
                    {
                        if (Math.Abs(nToY - nFromY) == 1 && nTarget == chess_type.blank)
                        {
                            return true;
                        }
                        else if (Math.Abs(nToY - nFromY) == 2 && nTarget == chess_type.small && chesses[(nFromX + nToX) / 2][(nFromY + nToY) / 2].type == chess_type.blank) 
                        {
                            return true; 
                        }
                    }
                    else if(nToY == nFromY)
                    {
                        if (Math.Abs(nToX - nFromX) == 1 && nTarget == chess_type.blank)
                        {
                            return true;
                        }
                        else if (Math.Abs(nToX - nFromX) == 2 && nTarget == chess_type.small && chesses[(nFromX + nToX) / 2][(nFromY + nToY) / 2].type == chess_type.blank)
                        {
                            return true;
                        }
                    }
                    break;
                case chess_type.small:
                    if (nToX == nFromX)
                    {
                        if (Math.Abs(nToY - nFromY) == 1 && nTarget == chess_type.blank)
                        {
                            return true;
                        }
                    }
                    else if (nToY == nFromY)
                    {
                        if (Math.Abs(nToX - nFromX) == 1 && nTarget == chess_type.blank)
                        {
                            return true;
                        }
                    }
                    break;
                default:
                    return false;   // The selected chess must be a big piece or a small piece.
            }
            return false;
        }

        private void AddStep(chess_type type, int nFromX, int nFromY, int nToX, int nToY, List<Step> moves)
        {
            Step step = new Step(type, new Position(nFromX, nFromY), new Position(nToX, nToY));
            moves.Add(step);
        }

        protected List<Step> GenerateAllMove(chess_type c)
        {
            List<Step> moves = new List<Step>();
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    if(board[i][j].type == c && board[i][j].type != chess_type.blank)
                    {
                        switch(board[i][j].type)
                        {
                            case chess_type.big:
                                GenerateMoveBig(i, j, c, moves);
                                break;
                            case chess_type.small:
                                GenerateMoveSmall(i, j, c, moves);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return moves;
        }

        private int GenerateMoveBig(int x, int y, chess_type c, List<Step> moves)
        {
            int bigMoves = 0;
            for (int i = x - 2; i <= x + 2; i++)
            {
                if (i < 0 || i > 4)
                    continue;
                for (int j = y - 2; j <= y + 2; j++)
                {
                    if (j < 0 || j > 4)
                        continue;
                    if (IsValidMove(board, x, y, i, j))
                    {
                        AddStep(c, x, y, i, j, moves);
                        bigMoves++;
                    }
                }
            }
            return bigMoves;
        }
        private int GenerateMoveSmall(int x,int y, chess_type c, List<Step> moves)
        {
            int smallMoves = 0;
            for (int i = x - 1; i <= x + 1; i++) 
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (IsValidMove(board, x, y, i, j))
                    {
                        AddStep(c, x, y, i, j, moves);
                        smallMoves++;
                    }
                }
            }
            return smallMoves;
        }

        private bool GameOver()
        {
            bool isOver = false;
            int smallCounts = 0;
            int bigMoves = 0;
            
            for (int i = 0; i < X; i++) 
            {
                for (int j = 0; j < Y; j++)
                {
                    switch(board[i][j].type)
                    {
                        case chess_type.big:
                            List<Step> moves = new List<Step>();
                            bigMoves += GenerateMoveBig(i, j, chess_type.big, moves);
                            break;
                        case chess_type.small:
                            smallCounts++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (smallCounts < 4 || bigMoves == 0)
                isOver = true;

            return isOver;
        }

        protected int GetStepNum(Step step)
        {
            return step.TargetPos.X * 10000 + step.TargetPos.Y * 1000 + step.SourcePos.X * 100 + step.SourcePos.Y * 10 + (int)step.type;
        }

        public Step CalcBestStep(chess_type type, int max_depth, bool clear_history)
        {
            if(clear_history)
            {
                history_score = new Dictionary<int, int>();
            }
            else
            {
                if(history_score == null)
                {
                    history_score = new Dictionary<int, int>();
                }
            }
            AlgorithmDepth = max_depth;
            AlphaBeta(type, AlgorithmDepth, -INFINITY, INFINITY, null);
            return BestStep;
        }

        private int AlphaBeta(chess_type type, int depth, int alpha, int beta, Step[] pre_steps)
        {
            if(GameOver())
            {
                return -INFINITY;
            }
            if (depth == 0)
            {
                return Evaluation(type, pre_steps);
            }

            List<Step> steps = GenerateAllMove(type);
            SortMove(steps);
            Step current_best_step = default(Step);
            foreach(var step in steps)
            {
                chess_type tarType = MakeMove(step);
                Step[] new_steps = null;
                if(pre_steps == null)
                {
                    new_steps = new Step[1];
                }
                else
                {
                    new_steps = new Step[pre_steps.Length + 1];
                    Array.Copy(pre_steps, new_steps, pre_steps.Length);
                }
                new_steps[^1] = step;

                int score = -AlphaBeta(SwitchType(type), depth - 1, -beta, -alpha, new_steps);
                UnMakeMove(step, tarType);

                if (score > alpha)
                {
                    alpha = score;
                    if (depth == AlgorithmDepth)
                    {
                        BestStep = step;
                    }

                    current_best_step = step;

                    if (alpha >= beta)
                        break;
                }
            }

            if (current_best_step != null)
            {
                int from_chess = GetStepNum(current_best_step);
                if (history_score.ContainsKey(from_chess))
                {
                    history_score[from_chess] += (1 << depth);
                }
                else
                {
                    history_score.Add(from_chess, (1 << depth));
                }
            }

            return alpha;
        }
        private chess_type SwitchType(chess_type type)
        {
            return type == chess_type.big ? chess_type.small : chess_type.big;
        }

        private int Evaluation(chess_type type, Step[] pre_steps)
        {
            int score = 0;
            for (int i = 0; i < X; i++) 
            {
                for (int j = 0; j < Y; j++)
                {
                    VirtualChess chess = board[i][j];
                    int temp_score = 0;
                    List<Step> moves = new List<Step>();
                    int count = 0;
                    switch(chess.type)
                    {
                        case chess_type.big:
                            temp_score = 0; // Need not set Big Piece value because of it can't be eat by Small Piece.
                            count = GenerateMoveBig(i, j, chess_type.big, moves);
                            temp_score += count * BIG_MOVE_VALUE;
                            break;
                        case chess_type.small:
                            temp_score = SMALL_VALUE;
                            count = GenerateMoveSmall(i, j, chess_type.small, moves);
                            temp_score += count * SMALL_MOVE_VALUE;
                            break;
                        case chess_type.blank:
                            temp_score = 0;
                            count = CheckLargePiece(i, j);
                            temp_score += count * BIG_MOVE_VALUE;
                            break;
                    }
                    if(chess.type == type)
                    {
                        score += temp_score;
                    }
                    else if(chess.type != type && chess.type != chess_type.blank)
                    {
                        score -= temp_score;
                    }
                    else
                    {
                        switch(type)
                        {
                            case chess_type.big:
                                if (count == 1)
                                    score += temp_score;
                                else if (count > 1 && count <= 3)
                                    score -= temp_score;
                                break;
                            case chess_type.small:
                                if (count == 1)
                                    score -= temp_score;
                                else if (count > 1 && count <= 3)
                                    score += temp_score;
                                break;
                        }
                    }
                }
            }
            return score;
        }
    }
}
