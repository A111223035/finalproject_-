using FinalProject_3016_3035;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        public GameForm(string playerName, string opponentName, string text1, string text2, string text3, System.Collections.Generic.List<string> listData)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.opponentName = opponentName;
            random = new Random();
            gameStarted = false;
            btnStartGame.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;

            // 設置目標控件的內容
            textBox1.Text = text1;
            textBox2.Text = text2;
            textBox3.Text = text3;

            // 將列表數據添加到 listBox1
            listBox1.Items.AddRange(listData.ToArray());

            InitializeGame();
        }

        public GameForm(string playerName, string opponentName)
        {
            this.playerName = playerName;
            this.opponentName = opponentName;
        }

        private void InitializeGame()
        {
            // 設定玩家選擇的按鈕
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
            btnRock.Click += BtnChoice_Click;
            btnPaper.Click += BtnChoice_Click;
            btnScissors.Click += BtnChoice_Click;

            // 設定對手選擇的按鈕
            btnRock2.Enabled = false;
            btnPaper2.Enabled = false;
            btnScissors2.Enabled = false;

            // 初始化倒數計時器
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            // 設定遊戲模式選擇按鈕的點擊事件
            button7.Click += (sender, e) => SetGameMode(3);  // 三戰兩勝
            button8.Click += (sender, e) => SetGameMode(5);  // 五戰三勝

            // 初始化遊戲狀態
            gameStarted = false;
            playerChoice = string.Empty;
            opponentChoice = string.Empty;
            playerScore = 0;
            opponentScore = 0;
            lblCountdown.Text = "剩餘選擇時間：5";
            txtResult.Text = "請選擇剪刀、石頭或布!";
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
        int OpponentChoice = 0;
        private string text1;
        private string text2;
        private string text3;
        private List<string> listData;

        private void BtnChoice_Click(object sender, EventArgs e)
        {
            if (!gameStarted) return;  // 如果遊戲尚未開始，則無法選擇

            playerChoice = (sender as Button).Text;
            opponentChoice = GetOpponentChoice();  // 獲得對手的隨機選擇
            StartCountdown();
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
            lblPlayerScore.Text = $"我方得分： {playerScore}";
            lblOpponentScore.Text = $"對手得分： {opponentScore}";

            // 檢查是否有獲勝條件
            if ((gamemode == 3 && playerScore == 2) || (gamemode == 5 && playerScore == 3))
            {
                MessageBox.Show(playerName + " 你贏了!");
                // 重置或處理遊戲結束邏輯
            }
            else if ((gamemode == 3 && opponentScore == 2) || (gamemode == 5 && opponentScore == 3))
            {
                MessageBox.Show(playerName + " 你輸了!");
                // 重置或處理遊戲結束邏輯
            }

            // 遊戲結束後禁用選擇按鈕
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
        }

        private string GetOpponentChoice()
        {
            string[] choices = { "剪刀", "石頭", "布" };
            return choices[random.Next(choices.Length)];
        }

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

        private void Listen()
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
                    inLen = T.ReceiveFrom(B, ref ServerEP);  // 接收伺服器發來的訊息
                }
                catch (Exception)
                {
                    T.Close();  // 關閉通訊器
                    listBox1.Items.Clear();  // 清除線上名單
                    MessageBox.Show("伺服器斷線了!");  // 顯示斷線消息
                    Th.Abort();  // 終止執行緒
                }

                Msg = Encoding.Default.GetString(B, 0, inLen);  // 解讀收到的訊息
                St = Msg.Substring(0, 1);  // 取得命令字（第一個字元）
                Str = Msg.Substring(1);  // 取得命令字後的訊息內容

                switch (St)
                {
                    // 處理遊戲開始訊息
                    case "L":
                        listBox1.Items.Clear();  // 清空現有玩家列表
                        string[] players = Str.Split(',');  // 將玩家名分割開
                        foreach (string player in players)
                        {
                            listBox1.Items.Add(player);  // 添加新玩家到列表
                        }
                        break;
                    case "R":
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
                    // 處理玩家選擇
                    case "2":
                        int opponentChoice = int.Parse(Str);  // 解析對手的選擇
                        if (opponentChoice == 1)
                        {
                            btnRock2.BackColor = Color.Yellow;
                        }
                        else if (opponentChoice == 2)
                        {
                            btnScissors2.BackColor = Color.Yellow;
                        }
                        else if (opponentChoice == 3)
                        {
                            btnPaper2.BackColor = Color.Yellow;
                        }
                        break;

                    // 處理「準備開始」指令
                    case "3":
                        if (Str == "start")
                        {
                            txtResult.Text = "遊戲開始!";
                            gameStarted = true;
                            StartCountdown();  // 開始倒數
                        }
                        break;

                    // 處理玩家回合結果
                    case "4":
                        // 顯示對方的選擇和回合結果
                        string[] resultParts = Str.Split(',');
                        string opponentMove = resultParts[0];  // 對手選擇的動作
                        string roundResult = resultParts[1];  // 回合結果
                        txtResult.Text = $"對手選擇了: {opponentMove}\n結果: {roundResult}";
                        break;

                    // 處理勝負更新指令
                    case "5":
                        string[] scoreParts = Str.Split(',');
                        int playerWins = int.Parse(scoreParts[0]);  // 玩家獲勝次數
                        int opponentWins = int.Parse(scoreParts[1]);  // 對手獲勝次數
                        lblPlayerScore.Text = $"我方得分： {playerWins}";
                        lblOpponentScore.Text = $"對手得分： {opponentWins}";
                        break;

                    // 處理選擇結果的勝負判斷
                    case "6":
                        int playerChoice = int.Parse(Str);  // 玩家選擇的動作
                        int OpponentChoice = int.Parse(Str);  // 對手選擇的動作

                        // 判斷勝負
                        if (playerChoice == OpponentChoice)
                        {
                            // 平手情況
                        }
                        else if ((playerChoice == 2 && OpponentChoice == 3) ||
                                 (playerChoice == 1 && OpponentChoice == 2) ||
                                 (playerChoice == 3 && OpponentChoice == 1) ||
                                 (playerChoice == 2 && OpponentChoice == 0) ||
                                 (playerChoice == 1 && OpponentChoice == 0) ||
                                 (playerChoice == 3 && OpponentChoice == 0))
                        {
                            playerScore++;  // 玩家獲勝，增加分數
                        }
                        else
                        {
                            opponentScore++;  // 對手獲勝，增加分數
                        }

                        // 更新玩家和對手選擇的按鈕顏色
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

                        // 根據對手的選擇更新顏色
                        if (OpponentChoice == 2)
                        {
                            btnScissors2.BackColor = Color.Yellow;
                        }
                        else if (OpponentChoice == 1)
                        {
                            btnRock2.BackColor = Color.Yellow;
                        }
                        else if (OpponentChoice == 3)
                        {
                            btnPaper2.BackColor = Color.Yellow;
                        }
                        break;

                    // 處理玩家成功連線的指令
                    case "7":
                        User = Str;  // 設置當前玩家的名稱
                        MessageBox.Show("成功連接到伺服器!");
                        break;

                    // 處理遊戲結束的指令
                    case "8":
                        string winner = Str;  // 獲勝玩家的名稱
                        MessageBox.Show(winner + " 獲得勝利!");
                        break;

                    // 預設情況，處理無法識別的指令
                    default:
                        Console.WriteLine("無法識別的命令: " + St);
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Send("9" + User);  // 在關閉表單前，通知伺服器用戶已離線。
            T.Close();        // 關閉套接字連線。
            // 創建 HomeForm 並傳遞當前的 Form
            HomeForm homeForm = new HomeForm();
            homeForm.Show(); // 顯示 HomeForm
            this.Hide();     // 隱藏 
        }

        private void Send(string Str) // 傳送訊息到伺服器
        {
            byte[] B = Encoding.Default.GetBytes(Str); // 將字串轉換為位元組陣列
            T.Send(B, 0, B.Length, SocketFlags.None); // 發送訊息
        }

        private void SetGameMode(int mode)
        {
            gamemode = mode; // 設定遊戲模式
            txtResult.Text = mode == 3 ? "三戰兩勝" : "五戰三勝"; // 顯示選擇的遊戲模式
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            button8.Enabled = false;

            txtResult.Text = "請選擇剪刀、石頭或布!";
            gameStarted = true;

            // 啟用玩家選擇的按鈕
            btnRock.Enabled = true;
            btnPaper.Enabled = true;
            btnScissors.Enabled = true;
            btnRock2.Enabled = false;
            btnPaper2.Enabled = false;
            btnScissors2.Enabled = false;

            // 重置倒數計時
            lblCountdown.Text = "剩餘選擇時間：5";
            countdown = 5;
            lblCountdown.Text = $"剩餘選擇時間： {countdown}";
            countdownTimer.Start();
        }

        // 關閉遊戲
        private void Forml_FormClosing(object sender, FormClosingEventArgs e)
        {
            Send("9" + User);  // 在關閉表單前，通知伺服器用戶已離線。
            T.Close();        // 關閉套接字連線。
            Application.ExitThread(); // 關閉應用程式
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gamemode = 3;
            button7.BackColor = Color.Yellow;
            button8.BackColor = original;
            btnStartGame.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            gamemode = 5;
            button7.BackColor = original; 
            button8.BackColor = Color.Yellow;
            btnStartGame.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Send("R" + "猜拳遊戲" + "|" + listBox1.SelectedItem);
            }
            btnPaper2.Enabled = false;
            btnScissors2.Enabled = false;
            btnRock2.Enabled = false;
            btnPaper.Enabled = false;
            btnRock.Enabled = false;
            btnScissors.Enabled = false;
            btnStartGame.Enabled = false;
            button7.Enabled = true;
            button8.Enabled = true;

            playerScore = 0;
            opponentScore = 0;
        }
    }
}
