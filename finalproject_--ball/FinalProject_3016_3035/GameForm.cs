using FinalProject_3016_3035;
using System;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class GameForm : Form
    {
        private string playerName;
        private string opponentName;
        private string playerChoice;
        private string opponentChoice;
        private Random random;
        private int countdown;
        private System.Windows.Forms.Timer countdownTimer;
        private bool gameStarted;

        // 分數變數
        private int playerScore = 0;
        private int opponentScore = 0;

        public GameForm(string playerName, string opponentName)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.opponentName = opponentName;
            random = new Random();
            gameStarted = false;

            InitializeGame();
        }

        private void InitializeGame()
        {
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
            btnStartGame.Click += BtnStartGame_Click;
            btnRock.Click += BtnChoice_Click;
            btnPaper.Click += BtnChoice_Click;
            btnScissors.Click += BtnChoice_Click;

            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            txtResult.Text = "請選擇剪刀、石頭或布!";
            gameStarted = true;

            // 啟用玩家選擇的按鈕
            btnRock.Enabled = true;
            btnPaper.Enabled = true;
            btnScissors.Enabled = true;

            // 重置倒數計時
            lblCountdown.Text = "倒計時: 0";
            countdown = 5;
            lblCountdown.Text = $"倒計時: {countdown}";
            countdownTimer.Start();
        }

        private void BtnChoice_Click(object sender, EventArgs e)
        {
            if (!gameStarted) return;  // 如果遊戲尚未開始，則無法選擇

            playerChoice = (sender as Button).Text;
            opponentChoice = GetOpponentChoice();  // 獲得對手的隨機選擇
            StartCountdown();
        }

        private string GetOpponentChoice()
        {
            string[] choices = { "剪刀", "石頭", "布" };
            return choices[random.Next(choices.Length)];
        }

        private void StartCountdown()
        {
            countdown = 5;
            lblCountdown.Text = $"倒計時: {countdown}";
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdown--;
            lblCountdown.Text = $"倒計時: {countdown}";

            if (countdown == 0)
            {
                countdownTimer.Stop();
                DetermineWinner();
            }
        }

        private void DetermineWinner()
        {
            string result;
            if (playerChoice == opponentChoice)
            {
                result = "平手!";
            }
            else if ((playerChoice == "剪刀" && opponentChoice == "布") ||
                     (playerChoice == "石頭" && opponentChoice == "剪刀") ||
                     (playerChoice == "布" && opponentChoice == "石頭"))
            {
                result = "你贏了!";
                playerScore++;  // 玩家贏時增加分數
            }
            else
            {
                result = "你輸了!";
                opponentScore++;  // 對手贏時增加分數
            }

            DisplayResult(result);
        }

        private void DisplayResult(string result)
        {
            txtResult.Text = $"你的選擇: {playerChoice}\n對手的選擇: {opponentChoice}\n結果: {result}";

            // 更新得分顯示
            lblPlayerScore.Text = $"你的分數: {playerScore}";
            lblOpponentScore.Text = $"對手的分數: {opponentScore}";

            // 遊戲結束後禁用選擇按鈕
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();  // 退出遊戲
        }

        private void Forml_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();  // 關閉應用程式
        }

        private void btnStartGame_Click_1(object sender, EventArgs e)
        {
            // 創建 GameForm 並傳遞當前的 Form1
            InviteFrom inviteForm = new InviteFrom(playerName, opponentName);
            inviteForm.Show(); // 顯示 GameForm
            this.Hide();     // 隱藏 Form1
        }
    }
}
