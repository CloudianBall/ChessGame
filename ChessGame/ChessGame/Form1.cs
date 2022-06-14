using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGame.SocketUtil;

namespace ChessGame
{
    public partial class Form1 : Form
    {

        public static player_type control_side;
        public bool selectedStatus = false;
        public Chess_piece SelectedPiece;
        public static Chess[][] Chesses;
        public static List<Chess> LargePieces;
        public static int SmallPieceCount;
        public Boolean isOver = false;
        public static List<Tuple<Chess, Chess>> records;    // Item1 is sourceChess, Item2 is targetChess.
        public static string testString = "";
        // DataBinding of RichTextBox control. If TestNotifyPropertyChanged's Text changed, RichTextBox content will change in time.
        public static TestNotifyPropertyChanged testTalkBoxText;
        delegate void SetTextCallback(string text);
        SocketServer server;
        SocketClient client;
        private player_type side;
        GameAI.GameAI gameAI;
        private bool isAIUse = false;
        public Form1()
        {
            InitializeComponent();

            testTalkBoxText = new TestNotifyPropertyChanged();
            testTalkBoxText.Text = "";

            TalkBox.DataBindings.Add("Text", testTalkBoxText, "Text");

            ResetRegion();
        }
        /// <summary>
        /// Set TalkBox Text in another thread.
        /// </summary>
        /// <param name="text"></param>
        public void SetTalkText(string text)
        {
            if(this.TalkBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTalkText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                testTalkBoxText.Text += text;
            }
        }

        private void ResetRegion()
        {
            Chesses = new Chess[5][];
            control_side = player_type.great;
            selectedStatus = false;
            SelectedPiece = null;
            ChessPanel.Controls.Clear();
            LargePieces = new List<Chess>();
            SmallPieceCount = 0;
            isOver = false;
            records = new List<Tuple<Chess, Chess>>();
            ControlSide.Text = "Large Control";
            side = player_type.blank;
            isAIUse = false;
            for (int i = 0; i < 5; i++)
            {
                Chesses[i] = new Chess[5];
                for (int j = 0; j < 5; j++)
                {
                    ButtonXY button = new ButtonXY(i, j);
                    if (j < 3)
                    {
                        button.Text = "S";
                        Chesses[i][j] = new Chess_piece(chess_type.small, player_type.little);
                        SmallPieceCount++;
                    }
                    else if (j == 4 && i % 2 == 0)
                    {
                        button.Text = "L";
                        Chesses[i][j] = new Chess_piece(chess_type.big, player_type.great);
                        LargePieces.Add(Chesses[i][j]);
                    }
                    else
                    {
                        button.Text = "";
                        Chesses[i][j] = new Chess_blank();
                    }
                    button.Height = 40;
                    button.Width = 40;
                    button.Click += Chess_Click;
                    ChessPanel.Controls.Add(button, i, j);
                    Chesses[i][j].PB = button;
                }
            }

            SmallCount.Text = "Small X " + SmallPieceCount;
        }
        private void SetControlSideText()
        {
            switch (control_side)
            {
                case player_type.blank:
                    ControlSide.Text = "  Control";
                    break;
                case player_type.great:
                    ControlSide.Text = "Large Control";
                    break;
                case player_type.little:
                    ControlSide.Text = "Small Control";
                    break;
            }
        }

        private void Chess_Click(object sender, EventArgs e)
        {
            if (side != control_side && (server != null || client != null)) 
            {
                return;
            }
            if (side != control_side && isAIUse) 
            {
                return;
            }
            ButtonXY button = sender as ButtonXY;
            int count = SmallPieceCount;
            if(selectedStatus)
            {
                if(SelectedPiece.Move_judge(button, SelectedPiece.PB.X, SelectedPiece.PB.Y, Chesses))
                {
                    selectedStatus = false;
                    SelectedPiece.Bg_toblank();
                    Win_Judge();
                    // Rotate control
                    if (!isOver)
                    {
                        if(button.X != SelectedPiece.PB.X || button.Y != SelectedPiece.PB.Y)
                            control_side = control_side == player_type.great ? player_type.little : player_type.great;
                        SetControlSideText();
                    }
                    if(server != null || client != null)
                    {
                        SendMsg(player_action.DoMove);
                    }
                    if(isAIUse)
                    {
                        DoAIMove();
                    }
                    if (count != SmallPieceCount)
                    {
                        SmallCount.Text = "Small X " + SmallPieceCount;
                    }
                }
                else
                {
                    if (button.X == SelectedPiece.PB.X && button.Y == SelectedPiece.PB.Y)
                    {
                        selectedStatus = false;
                        SelectedPiece.Bg_toblank();
                    }
                }
            }
            else
            {
                SelectedPiece = Chesses[button.X][button.Y] is Chess_piece ? Chesses[button.X][button.Y] as Chess_piece : null;
                // Turns the currently selected piece that belongs to the controlling to red
                if (SelectedPiece != null && SelectedPiece.side == control_side)
                {
                    SelectedPiece.Bg_toRed();
                    selectedStatus = true;
                }
                else
                    selectedStatus = false;
            }
        }

        private void DoAIMove()
        {
            if (records.Count > 0)
                gameAI.UpdateBoard(records.Last());
            chess_type type = gameAI.AiSide == player_type.great ? chess_type.big : chess_type.small;
            GameAI.Step aiMove = gameAI.CalcBestStep(type, 3, false);
            if (aiMove.type == Chesses[aiMove.SourcePos.X][aiMove.SourcePos.Y].type && aiMove.type != chess_type.blank)
            {
                if (Chesses[aiMove.SourcePos.X][aiMove.SourcePos.Y].Move_judge(Chesses[aiMove.TargetPos.X][aiMove.TargetPos.Y].PB, aiMove.SourcePos.X, aiMove.SourcePos.Y, Chesses))
                {
                    Win_Judge();
                    // Rotate control
                    if (!isOver)
                    {
                        control_side = control_side == player_type.great ? player_type.little : player_type.great;
                        SetControlSideText();
                        gameAI.UpdateBoard(records.Last());
                    }
                }
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确认重开一局？", "提示", mess);
            if (dr == DialogResult.OK)
            {
                ResetRegion();
            }
        }

        private void DoSurrender()
        {
            MessageBoxButtons mess = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确认投降？", "提示", mess);
            if (dr == DialogResult.OK)
            {
                string win_side = control_side == player_type.great ? "S" : "L";
                if (server != null || client != null)
                {
                    SendMsg(player_action.DoSurrender);
                    CloseHost();
                }
                MessageBox.Show(win_side + " 方获胜！");
                control_side = player_type.blank;
                SetControlSideText();
                isOver = true;
            }
        }
        private void ReceiveSurrender()
        {
            string win_side = side == player_type.great ? "L" : "S";
            MessageBox.Show(win_side + " 方获胜！");
            CloseHost();
        }

        private void surrender_Click(object sender, EventArgs e)
        {
            DoSurrender();
        }

        /// <summary>
        /// Check large pieces surroundings to judge if failed.
        /// </summary>
        /// <returns>True is not over, False is over.</returns>
        private bool CheckLargeSurroundings()
        {
            int count = 0;
            int i, j;
            foreach(var piece in LargePieces)
            {
                int x = piece.PB.X;
                int y = piece.PB.Y;
                for (i = x - 1; i <= x + 1; i++)
                {
                    if (i < 0 || i >= 5)
                        continue;
                    for (j = y - 1; j <= y + 1; j++)
                    {
                        if (j < 0 || j >= 5)
                            continue;
                        if (i == x && j == y)
                            continue;
                        if (Math.Abs(i - x) == 1 && Math.Abs(j - y) == 1)
                            continue;
                        if (Chesses[i][j].type == chess_type.blank)
                            break;
                    }
                    if (j != y + 2)
                        break;
                }
                if (i != x + 2)
                {
                    count++;
                    break;
                }
            }
            if (count == 0)
                return false;
            else
                return true;
        }

        private void Win_Judge()
        {
            player_type winSide = player_type.blank;
            switch(control_side)
            {
                case player_type.great:
                    if (SmallPieceCount < 4) 
                    {
                        isOver = true;
                        winSide = player_type.great;
                    }
                    break;
                case player_type.little:
                    if(!CheckLargeSurroundings())
                    {
                        isOver = true;
                        winSide = player_type.little;
                    }
                    break;
                case player_type.blank:
                    break;
            }

            if(isOver)
            {
                control_side = player_type.blank;
                SetControlSideText();
                switch (winSide)
                {
                    case player_type.great:
                        MessageBox.Show("小棋子一方不足4个棋子，大的一方获胜");
                        break;
                    case player_type.little:
                        MessageBox.Show("大棋子一方不能移动，小的一方获胜");
                        break;
                    default:
                        break;
                }
                if (server != null || client != null)
                {
                    SendMsg(player_action.DoMove);
                    CloseHost();
                }
            }
        }

        private void DoRegret()
        {
            if (records.Count >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    int count = SmallPieceCount;
                    var lastResource = records[records.Count - 1].Item1;
                    var lastTarget = records[records.Count - 1].Item2;
                    switch (lastResource.type)
                    {
                        case chess_type.big:
                            Chesses[lastResource.PB.X][lastResource.PB.Y].ToBig(ref Chesses[lastResource.PB.X][lastResource.PB.Y]);
                            break;
                        case chess_type.small:
                            Chesses[lastResource.PB.X][lastResource.PB.Y].ToSmall(ref Chesses[lastResource.PB.X][lastResource.PB.Y]);
                            break;
                        case chess_type.blank:
                            Chesses[lastResource.PB.X][lastResource.PB.Y].ToBlank(ref Chesses[lastResource.PB.X][lastResource.PB.Y]);
                            break;
                    }
                    switch (lastTarget.type)
                    {
                        case chess_type.big:
                            Chesses[lastTarget.PB.X][lastTarget.PB.Y].ToBig(ref Chesses[lastTarget.PB.X][lastTarget.PB.Y]);
                            break;
                        case chess_type.small:
                            Chesses[lastTarget.PB.X][lastTarget.PB.Y].ToSmall(ref Chesses[lastTarget.PB.X][lastTarget.PB.Y]);
                            SmallPieceCount++;
                            break;
                        case chess_type.blank:
                            Chesses[lastTarget.PB.X][lastTarget.PB.Y].ToBlank(ref Chesses[lastTarget.PB.X][lastTarget.PB.Y]);
                            break;
                    }
                    if (count != SmallPieceCount)
                    {
                        SmallCount.Text = "Small X " + SmallPieceCount;
                    }
                    records.RemoveAt(records.Count - 1);
                    control_side = control_side == player_type.great ? player_type.little : player_type.great;
                    SetControlSideText();
                }
            }
        }

        private void Regret_Click(object sender, EventArgs e)
        {
            DoRegret();
            if (server != null || client != null)
            {
                SendMsg(player_action.DoRegret);
            }
            if (isAIUse)
            {
                gameAI.InitBoard();
            }
        }

        private void CreateServer_Click(object sender, EventArgs e)
        {
            ResetRegion();
            side = player_type.great;
            server = new SocketServer(8888,this);
            server.StartListen();
            CreateClient.Enabled = false;
            CreateServer.Enabled = false;
            ExitHost.Enabled = true;
        }

        private void CreateClient_Click(object sender, EventArgs e)
        {
            ResetRegion();
            side = player_type.little;
            client = new SocketClient(8888, this);
            client.StartClient();
            CreateClient.Enabled = false;
            CreateServer.Enabled = false;
            ExitHost.Enabled = true;
        }

        private void SendMsg(player_action action, string message = "")
        {
            string msg;
            switch(action)
            {
                case player_action.DoMove:  // Send the source piece and target piece info to opposite
                    msg = String.Format("DoMove;{0},{1},{2},{3},{4};{5},{6},{7},{8},{9}",
                        records.Last().Item1.type, records.Last().Item1.side, records.Last().Item1.PB.X, records.Last().Item1.PB.Y, records.Last().Item1.PB.Text,
                        records.Last().Item2.type, records.Last().Item2.side, records.Last().Item2.PB.X, records.Last().Item2.PB.Y, records.Last().Item2.PB.Text);                    
                    break;
                case player_action.DoMsg:   // Chat is not implemented in UI.
                    msg = String.Format("DoMsg;{0}",message);
                    break;
                case player_action.DoRegret:
                    msg = String.Format("DoRegret;");
                    break;
                case player_action.DoSurrender:
                    msg = String.Format("DoSurrender;");
                    break;
                default:
                    msg = String.Format("DoMsg;{0}", message);
                    break;
            }
            if (server != null)
                server.SendMessage(msg);
            else if (client != null)
                client.SendMessage(msg);
        }
        public void ProcessMsg(string msg)
        {
            var strs = msg.Split(';');
            player_action action;
            if(!Enum.TryParse(strs[0], out action))
            {
                return;
            }

            if (this.TalkBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ProcessMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                switch (action)
                {
                    case player_action.DoMove:
                        var source = strs[1].Split(',');
                        int sourceX = int.Parse(source[2]);
                        int sourceY = int.Parse(source[3]);
                        var target = strs[2].Split(',');
                        int targetX = int.Parse(target[2]);
                        int targetY = int.Parse(target[3]);
                        SelectedPiece = Chesses[sourceX][sourceY] is Chess_piece ? Chesses[sourceX][sourceY] as Chess_piece : null;
                        ButtonXY targetButton = new ButtonXY(targetX, targetY);
                        int count = SmallPieceCount;
                        if (SelectedPiece.Move_judge(targetButton, sourceX, sourceY, Chesses))
                        {
                            selectedStatus = false;
                            SelectedPiece.Bg_toblank();
                            Win_Judge();
                            // Rotate Control
                            if (!isOver)
                            {
                                control_side = control_side == player_type.great ? player_type.little : player_type.great;
                                SetControlSideText();
                            }
                            if (count != SmallPieceCount)
                            {
                                SmallCount.Text = "Small X " + SmallPieceCount;
                            }
                        }
                        break;
                    case player_action.DoMsg:
                        break;
                    case player_action.DoRegret:
                        DoRegret();
                        break;
                    case player_action.DoSurrender:
                        ReceiveSurrender();
                        break;
                    default:
                        break;
                }
            }
        }

        private void TalkBox_TextChanged(object sender, EventArgs e)
        {
            TalkBox.SelectionStart = TalkBox.Text.Length;
            TalkBox.ScrollToCaret();
        }

        private void ExitHost_Click(object sender, EventArgs e)
        {
            if (server != null || client != null)
            {
                SendMsg(player_action.DoMsg, "已退出房间");
            }
            CloseHost();
        }

        private void CloseHost()
        {
            if (this.server != null)
            {
                this.server.Close();
            }
            if (this.client != null)
            {
                this.client.Close();
            }
            CreateServer.Enabled = true;
            CreateClient.Enabled = true;
            ExitHost.Enabled = false;
        }

        private void SelectBig_Click(object sender, EventArgs e)
        {
            ResetRegion();
            side = player_type.great;
            gameAI = new GameAI.GameAI(5, 5, 3, player_type.little);
            isAIUse = true;
        }

        private void SelectSmall_Click(object sender, EventArgs e)
        {
            ResetRegion();
            side = player_type.little;
            gameAI = new GameAI.GameAI(5, 5, 3, player_type.great);
            isAIUse = true;
            DoAIMove();
        }
    }
}
