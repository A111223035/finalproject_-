using FinalProject_3016_3035;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private int PlayerChoice;


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

            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            btnPaper.Enabled = false;
            btnRock.Enabled = false;
            btnScissors.Enabled = false;
            btnStartGame.Enabled = true;
        }
        // 宣告變數
        Socket T; // 用於網絡通信的 Socket
        Thread Th; // 用於接收數據的執行緒
        string User; // 儲存用戶名稱
        Color original; // 儲存按鈕的初始背景顏色
        int win = 0; // 記錄勝利次數
        int lose = 0; // 記錄失敗次數
        int gamemode = 0; // 記錄遊戲模式
        int turn = 0; // 記錄回合數
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
            lblCountdown.Text = "剩餘選擇時間： 0";
            countdown = 5;
            lblCountdown.Text = $"剩餘選擇時間： {countdown}";
            countdownTimer.Start();
        }

        private void BtnChoice_Click(object sender, EventArgs e)
        {
            if (!gameStarted) return;  // 如果遊戲尚未開始，則無法選擇

            playerChoice = (sender as System.Windows.Forms.Button).Text;
 //           opponentChoice = GetOpponentChoice();  // 獲得對手的隨機選擇            
        }

//        private string GetOpponentChoice()
//        {
//           string[] choices = { "剪刀", "石頭", "布" };
//            return choices[random.Next(choices.Length)];
//        }

        private void StartCountdown()
        {
            countdown = 5;
            lblCountdown.Text = $"剩餘選擇時間： {countdown}";
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdown--;
            lblCountdown.Text = $"剩餘選擇時間： {countdown}";

            if (countdown == 0)
            {
                countdownTimer.Stop();
                DetermineWinner();
            }
        }

        private void DetermineWinner()
        {

            Send("6" + "|" + listBox1.SelectedItem);
            if (gamemode == 3)
                if (playerScore == 2)
                {
                    MessageBox.Show(User + "你贏了");
                    Send("4" + win + "|" + listBox1.SelectedItem);
                }

            if (gamemode == 5)
                if (playerScore == 3)
                {
                    MessageBox.Show(User + "你贏了");
                    Send("4" + win + "|" + listBox1.SelectedItem);
                }
        }


        private void DisplayResult(string result)
        {
            txtResult.Text = $"你的選擇: {playerChoice}\n對手的選擇: {opponentChoice}\n結果: {result}";

            // 更新得分顯示
            lblPlayerScore.Text = $"我方得分： {playerScore}";
            lblOpponentScore.Text = $"對手得分： {opponentScore}";

            // 檢查是否有獲勝條件
            if (gamemode == 3 && playerScore == 2)
            {
                MessageBox.Show(User + "你贏了"); // 顯示勝利訊息
                Send("4" + win + "|" + listBox1.SelectedItem); // 發送勝利訊息給伺服器
            }

            if (gamemode == 5 && playerScore == 3)
            {
                MessageBox.Show(User + "你贏了"); // 顯示勝利訊息
                Send("4" + win + "|" + listBox1.SelectedItem); // 發送勝利訊息給伺服器
            }
            
            if (gamemode == 3 && opponentScore == 2)
            {
                MessageBox.Show(User + "你輸了"); // 顯示勝利訊息
                Send("4" + win + "|" + listBox1.SelectedItem); // 發送勝利訊息給伺服器
            }

            if (gamemode == 5 && opponentScore == 3)
            {
                MessageBox.Show(User + "你輸了"); // 顯示勝利訊息
                Send("4" + win + "|" + listBox1.SelectedItem); // 發送勝利訊息給伺服器
            }


            // 遊戲結束後禁用選擇按鈕
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
        }

        private void Forml_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();  // 關閉應用程式
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            button4.BackColor = original; // 恢復按鈕背景顏色
            button5.BackColor = original; // 恢復按鈕背景顏色
            button6.BackColor = original; // 恢復按鈕背景顏色
            btnPaper.BackColor = original; // 恢復按鈕背景顏色
            btnRock.BackColor = original; // 恢復按鈕背景顏色
            btnScissors.BackColor = original; // 恢復按鈕背景顏色
            button2.Enabled = true;
            button3.Enabled = true;
            btnPaper.Enabled = true;
            btnRock.Enabled = true;
            btnScissors.Enabled = true;
            StartCountdown();
        }

        private void Send(string Str) // 傳送訊息到伺服器
        {
            byte[] B = Encoding.Default.GetBytes(Str); // 將字串轉換為位元組陣列
            T.Send(B, 0, B.Length, SocketFlags.None); // 發送訊息
        }

        private void Listen() // 監聽伺服器回應的執行緒方法
        {
            EndPoint ServerEP = (EndPoint)T.RemoteEndPoint;
            byte[] B = new byte[1023];
            int inLen = 0;
            string Msg;
            string St;
            string Str;
            while (true)
            {
                try
                {
                    inLen = T.ReceiveFrom(B, ref ServerEP);
                }
                catch (Exception)
                {
                    T.Close();
                    listBox1.Items.Clear();
                    MessageBox.Show("伺服器斷線了!");
                    Th.Abort();
                }
                Msg = Encoding.Default.GetString(B, 0, inLen);
                St = Msg.Substring(0, 1);
                Str = Msg.Substring(1);
                switch (St)
                {
                    case "L":
                        listBox1.Items.Clear();
                        string[] M = Str.Split(',');
                        for (int i = 0; i < M.Length; i++) listBox1.Items.Add(M[i]);
                        break;
                    case "5":
                        DialogResult result = MessageBox.Show("是否重玩遊戲(" + Str + ")?", "重玩訊息", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
//                            comboBox1.Text = Str;
//                            comboBox1.Enabled = false;
                            Send("P" + "Y" + "|" + listBox1.SelectedItem);
                        }
                        else
                        {
                            Send("P" + "N" + "|" + listBox1.SelectedItem);
                        }
                        break;
                    case "P":
                        if (Str == "Y")
                        {
                            MessageBox.Show(listBox1.SelectedItem.ToString() + "接受你的邀請，可以開始重玩遊戲");
                        }
                        else
                        {
                            MessageBox.Show("抱歉" + listBox1.SelectedItem.ToString() + "拒絕你的邀請");
                        }
                        break;
                    case "6":
                        int opponentChoice = int.Parse(Str);
                        int playerChoice = PlayerChoice;

                        if (playerChoice == opponentChoice)
                        {
                        }
                        else if ((playerChoice == 2 && opponentChoice == 3) ||
                                 (playerChoice == 1 && opponentChoice == 2) ||
                                 (playerChoice == 3 && opponentChoice == 1) ||
                                 (playerChoice == 2 && opponentChoice == 0) ||
                                 (playerChoice == 1 && opponentChoice == 0) ||
                                 (playerChoice == 3 && opponentChoice == 0) )
                        {
                            playerScore++;  // 玩家贏時增加分數
                        }
                        else
                        {
                            opponentScore++;  // 對手贏時增加分數
                        }

                        if (playerChoice == 2)
                        {
                            btnScissors.BackColor = Color.Yellow;
                        }
                        else if (playerChoice == 1)
                        {
                            btnRock.BackColor = Color.Yellow;
                        }
                        else if (playerChoice == 3)
                        {
                            btnPaper.BackColor = Color.Yellow;
                        }

                        if (opponentChoice == 2)
                        {
                            button4.BackColor = Color.Yellow;
                        }
                        else if (opponentChoice == 1)
                        {
                            button6.BackColor = Color.Yellow;
                        }
                        else if (opponentChoice == 3)
                        {
                            button5.BackColor = Color.Yellow;
                        }
                        break;
                    case "4":
                        int opwins = int.Parse(Str);
                        if (opwins > win)
                            MessageBox.Show(User + "你輸了");
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Send("5" + "猜拳遊戲" + "|" + listBox1.SelectedItem);
            }
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            btnPaper.Enabled = false;
            btnRock.Enabled = false;
            btnScissors.Enabled = false;
            btnStartGame.Enabled = true;

            playerScore = 0;
            opponentScore = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 創建 HomeForm 並傳遞當前的 Form
            HomeForm homeForm = new HomeForm();
            homeForm.Show(); // 顯示 HomeForm
            this.Hide();     // 隱藏 
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            PlayerChoice = 1;
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            PlayerChoice = 2;
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            PlayerChoice = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gamemode = 3;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            gamemode = 5;
        }
    }
}
