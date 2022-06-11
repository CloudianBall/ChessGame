namespace ChessGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if(this.server != null)
            {
                this.server.Close();
            }
            if(this.client != null)
            {
                this.client.Close();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChessPanel = new System.Windows.Forms.TableLayoutPanel();
            this.reset = new System.Windows.Forms.Button();
            this.surrender = new System.Windows.Forms.Button();
            this.Regret = new System.Windows.Forms.Button();
            this.ControlSide = new System.Windows.Forms.Label();
            this.SmallCount = new System.Windows.Forms.Label();
            this.CreateServer = new System.Windows.Forms.Button();
            this.CreateClient = new System.Windows.Forms.Button();
            this.TalkBox = new System.Windows.Forms.RichTextBox();
            this.ExitHost = new System.Windows.Forms.Button();
            this.SelectBig = new System.Windows.Forms.Button();
            this.SelectSmall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChessPanel
            // 
            this.ChessPanel.ColumnCount = 5;
            this.ChessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.Location = new System.Drawing.Point(241, 72);
            this.ChessPanel.Name = "ChessPanel";
            this.ChessPanel.RowCount = 5;
            this.ChessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ChessPanel.Size = new System.Drawing.Size(250, 250);
            this.ChessPanel.TabIndex = 0;
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(241, 347);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(75, 23);
            this.reset.TabIndex = 1;
            this.reset.Text = "重开";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // surrender
            // 
            this.surrender.Location = new System.Drawing.Point(416, 347);
            this.surrender.Name = "surrender";
            this.surrender.Size = new System.Drawing.Size(75, 23);
            this.surrender.TabIndex = 2;
            this.surrender.Text = "投降";
            this.surrender.UseVisualStyleBackColor = true;
            this.surrender.Click += new System.EventHandler(this.surrender_Click);
            // 
            // Regret
            // 
            this.Regret.Location = new System.Drawing.Point(497, 111);
            this.Regret.Name = "Regret";
            this.Regret.Size = new System.Drawing.Size(75, 23);
            this.Regret.TabIndex = 3;
            this.Regret.Text = "悔棋";
            this.Regret.UseVisualStyleBackColor = true;
            this.Regret.Click += new System.EventHandler(this.Regret_Click);
            // 
            // ControlSide
            // 
            this.ControlSide.AutoSize = true;
            this.ControlSide.Location = new System.Drawing.Point(325, 9);
            this.ControlSide.Name = "ControlSide";
            this.ControlSide.Size = new System.Drawing.Size(88, 17);
            this.ControlSide.TabIndex = 4;
            this.ControlSide.Text = "Large Control";
            // 
            // SmallCount
            // 
            this.SmallCount.AutoSize = true;
            this.SmallCount.Location = new System.Drawing.Point(497, 72);
            this.SmallCount.Name = "SmallCount";
            this.SmallCount.Size = new System.Drawing.Size(87, 17);
            this.SmallCount.TabIndex = 5;
            this.SmallCount.Text = "Small X count";
            // 
            // CreateServer
            // 
            this.CreateServer.Location = new System.Drawing.Point(12, 9);
            this.CreateServer.Name = "CreateServer";
            this.CreateServer.Size = new System.Drawing.Size(75, 23);
            this.CreateServer.TabIndex = 6;
            this.CreateServer.Text = "创建主机";
            this.CreateServer.UseVisualStyleBackColor = true;
            this.CreateServer.Click += new System.EventHandler(this.CreateServer_Click);
            // 
            // CreateClient
            // 
            this.CreateClient.Location = new System.Drawing.Point(93, 9);
            this.CreateClient.Name = "CreateClient";
            this.CreateClient.Size = new System.Drawing.Size(75, 23);
            this.CreateClient.TabIndex = 7;
            this.CreateClient.Text = "加入主机";
            this.CreateClient.UseVisualStyleBackColor = true;
            this.CreateClient.Click += new System.EventHandler(this.CreateClient_Click);
            // 
            // TalkBox
            // 
            this.TalkBox.Location = new System.Drawing.Point(12, 72);
            this.TalkBox.Name = "TalkBox";
            this.TalkBox.Size = new System.Drawing.Size(223, 298);
            this.TalkBox.TabIndex = 8;
            this.TalkBox.Text = "Test";
            this.TalkBox.TextChanged += new System.EventHandler(this.TalkBox_TextChanged);
            // 
            // ExitHost
            // 
            this.ExitHost.Enabled = false;
            this.ExitHost.Location = new System.Drawing.Point(174, 9);
            this.ExitHost.Name = "ExitHost";
            this.ExitHost.Size = new System.Drawing.Size(75, 23);
            this.ExitHost.TabIndex = 9;
            this.ExitHost.Text = "退出主机";
            this.ExitHost.UseVisualStyleBackColor = true;
            this.ExitHost.Click += new System.EventHandler(this.ExitHost_Click);
            // 
            // SelectBig
            // 
            this.SelectBig.Location = new System.Drawing.Point(497, 3);
            this.SelectBig.Name = "SelectBig";
            this.SelectBig.Size = new System.Drawing.Size(75, 23);
            this.SelectBig.TabIndex = 10;
            this.SelectBig.Text = "选择大";
            this.SelectBig.UseVisualStyleBackColor = true;
            this.SelectBig.Click += new System.EventHandler(this.SelectBig_Click);
            // 
            // SelectSmall
            // 
            this.SelectSmall.Location = new System.Drawing.Point(578, 3);
            this.SelectSmall.Name = "SelectSmall";
            this.SelectSmall.Size = new System.Drawing.Size(75, 23);
            this.SelectSmall.TabIndex = 10;
            this.SelectSmall.Text = "选择小";
            this.SelectSmall.UseVisualStyleBackColor = true;
            this.SelectSmall.Click += new System.EventHandler(this.SelectSmall_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SelectSmall);
            this.Controls.Add(this.SelectBig);
            this.Controls.Add(this.ExitHost);
            this.Controls.Add(this.TalkBox);
            this.Controls.Add(this.CreateClient);
            this.Controls.Add(this.CreateServer);
            this.Controls.Add(this.SmallCount);
            this.Controls.Add(this.ControlSide);
            this.Controls.Add(this.Regret);
            this.Controls.Add(this.surrender);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.ChessPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ChessPanel;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button surrender;
        private System.Windows.Forms.Button Regret;
        private System.Windows.Forms.Label ControlSide;
        private System.Windows.Forms.Label SmallCount;
        private System.Windows.Forms.Button CreateServer;
        private System.Windows.Forms.Button CreateClient;
        private System.Windows.Forms.RichTextBox TalkBox;
        private System.Windows.Forms.Button ExitHost;
        private System.Windows.Forms.Button SelectBig;
        private System.Windows.Forms.Button SelectSmall;
    }
}

