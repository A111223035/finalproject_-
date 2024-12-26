namespace FinalProject
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.txtSystemMessage = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.btnScissors = new System.Windows.Forms.Button();
            this.btnPaper = new System.Windows.Forms.Button();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.btnRock = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblPlayerScore = new System.Windows.Forms.Label();
            this.lblOpponentScore = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSystemMessage
            // 
            this.txtSystemMessage.AutoSize = true;
            this.txtSystemMessage.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSystemMessage.Location = new System.Drawing.Point(133, 338);
            this.txtSystemMessage.Name = "txtSystemMessage";
            this.txtSystemMessage.Size = new System.Drawing.Size(86, 24);
            this.txtSystemMessage.TabIndex = 204;
            this.txtSystemMessage.Text = "系統訊息";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(407, 132);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(109, 104);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 203;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(272, 132);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(109, 104);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 202;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(141, 132);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 201;
            this.pictureBox1.TabStop = false;
            // 
            // txtResult
            // 
            this.txtResult.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtResult.Location = new System.Drawing.Point(227, 334);
            this.txtResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(371, 32);
            this.txtResult.TabIndex = 200;
            // 
            // btnStartGame
            // 
            this.btnStartGame.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnStartGame.Location = new System.Drawing.Point(549, 244);
            this.btnStartGame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(123, 41);
            this.btnStartGame.TabIndex = 199;
            this.btnStartGame.Text = "開始遊戲";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click_1);
            // 
            // btnScissors
            // 
            this.btnScissors.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnScissors.Location = new System.Drawing.Point(288, 254);
            this.btnScissors.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnScissors.Name = "btnScissors";
            this.btnScissors.Size = new System.Drawing.Size(84, 41);
            this.btnScissors.TabIndex = 198;
            this.btnScissors.Text = "剪刀 ";
            this.btnScissors.UseVisualStyleBackColor = true;
            // 
            // btnPaper
            // 
            this.btnPaper.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnPaper.Location = new System.Drawing.Point(421, 254);
            this.btnPaper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPaper.Name = "btnPaper";
            this.btnPaper.Size = new System.Drawing.Size(84, 41);
            this.btnPaper.TabIndex = 197;
            this.btnPaper.Text = "布";
            this.btnPaper.UseVisualStyleBackColor = true;
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCountdown.Location = new System.Drawing.Point(559, 198);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(65, 24);
            this.lblCountdown.TabIndex = 196;
            this.lblCountdown.Text = "label3";
            // 
            // btnRock
            // 
            this.btnRock.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRock.Location = new System.Drawing.Point(155, 254);
            this.btnRock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRock.Name = "btnRock";
            this.btnRock.Size = new System.Drawing.Size(83, 41);
            this.btnRock.TabIndex = 195;
            this.btnRock.Text = "石頭";
            this.btnRock.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(613, 342);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 24);
            this.button1.TabIndex = 205;
            this.button1.Text = "退出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblPlayerScore
            // 
            this.lblPlayerScore.AutoSize = true;
            this.lblPlayerScore.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPlayerScore.Location = new System.Drawing.Point(559, 98);
            this.lblPlayerScore.Name = "lblPlayerScore";
            this.lblPlayerScore.Size = new System.Drawing.Size(21, 24);
            this.lblPlayerScore.TabIndex = 206;
            this.lblPlayerScore.Text = "0";
            // 
            // lblOpponentScore
            // 
            this.lblOpponentScore.AutoSize = true;
            this.lblOpponentScore.Font = new System.Drawing.Font("微軟正黑體", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblOpponentScore.Location = new System.Drawing.Point(559, 132);
            this.lblOpponentScore.Name = "lblOpponentScore";
            this.lblOpponentScore.Size = new System.Drawing.Size(21, 24);
            this.lblOpponentScore.TabIndex = 207;
            this.lblOpponentScore.Text = "0";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblOpponentScore);
            this.Controls.Add(this.lblPlayerScore);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSystemMessage);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.btnScissors);
            this.Controls.Add(this.btnPaper);
            this.Controls.Add(this.lblCountdown);
            this.Controls.Add(this.btnRock);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Forml_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label txtSystemMessage;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Button btnScissors;
        private System.Windows.Forms.Button btnPaper;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Button btnRock;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblPlayerScore;
        private System.Windows.Forms.Label lblOpponentScore;
        private System.Windows.Forms.Timer timer1;
    }
}