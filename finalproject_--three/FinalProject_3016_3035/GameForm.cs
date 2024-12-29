using FinalProject_3016_3035;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

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

            // 設置目標控件的內容
            textBox1.Text = text1;
            textBox2.Text = text2;
            textBox3.Text = text3;

            // 將列表數據添加到 listBox1
            listBox1.Items.AddRange(listData.ToArray());

            InitializeGame();
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
            lblCountdown.Text = "倒計時: 0";
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
        private void DetermineAndDisplayWinner()
        {
            string result;

            // 判斷勝負
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

            // 顯示結果
            txtResult.Text = $"你的選擇: {playerChoice}\n對手的選擇: {opponentChoice}\n結果: {result}";
            lblPlayerScore.Text = $"我方得分： {playerScore}";
            lblOpponentScore.Text = $"對手得分： {opponentScore}";

            // 檢查勝利條件
            if ((gamemode == 3 && playerScore == 2) || (gamemode == 5 && playerScore == 3))
            {
                MessageBox.Show(playerName + " 你贏了!");
                // 重置遊戲或處理結束邏輯
            }
            else if ((gamemode == 3 && opponentScore == 2) || (gamemode == 5 && opponentScore == 3))
            {
                MessageBox.Show(playerName + " 你輸了!");
                // 重置遊戲或處理結束邏輯
            }

            // 禁用選擇按鈕
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();  // 退出遊戲
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem.ToString() != User)//有選擇對手
                {
                    Send("I" + User + "," + "|" + listBox1.SelectedItem);
                }
                else
                {
                    MessageBox.Show("不可以邀請自己!");
                }
            }
            else
            {
                MessageBox.Show("沒有選取邀請的對象!");//如果沒有選擇對手
            }
        }

        private void Forml_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();  // 關閉應用程式
        }

        private void SetGameMode(int mode)
        {
            gamemode = mode;  // 設定遊戲模式
            txtResult.Text = mode == 3 ? "三戰兩勝" : "五戰三勝"; // 顯示選擇的遊戲模式
        }


        private void BtnStartGame_Click(object sender, EventArgs e)
        {
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
            lblCountdown.Text = "倒計時: 0";
            countdown = 5;
            lblCountdown.Text = $"倒計時: {countdown}";
            countdownTimer.Start();
        }

//            btnScissors2.BackColor = original; // 恢復按鈕背景顏色
//            btnPaper2.BackColor = original; // 恢復按鈕背景顏色
//            btnRock2.BackColor = original; // 恢復按鈕背景顏色
//            btnPaper.BackColor = original; // 恢復按鈕背景顏色
//            btnRock.BackColor = original; // 恢復按鈕背景顏色
//            btnScissors.BackColor = original; // 恢復按鈕背景顏色
//           // 創建 GameForm 並傳遞當前的 Form1
//            InviteFrom inviteForm = new InviteFrom(playerName, opponentName);
//            inviteForm.Show(); // 顯示 GameForm


        private void Send(string Str) // 傳送訊息到伺服器
        {
            byte[] B = Encoding.Default.GetBytes(Str); // 將字串轉換為位元組陣列
            T.Send(B, 0, B.Length, SocketFlags.None); // 發送訊息
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
                    inLen = T.ReceiveFrom(B, ref ServerEP);
                }
                catch (Exception)
                {
                    T.Close();//關閉通訊器
                    listBox1.Items.Clear();//清除線上名單
                    MessageBox.Show("伺服器斷線了!");//顯示斷線
                    button1.Enabled = true;//連線按鍵恢復可用
                    Th.Abort();//刪除執行緒
                }
                Msg = Encoding.Default.GetString(B, 0, inLen);//解讀完整訊息
                St = Msg.Substring(0, 1);//取出命令碼 (第一個字)
                Str = Msg.Substring(1);//取出命令碼之後的訊息
                switch (St)//依命令碼執行功能
                {
                    case "L"://接收線上名單
                        listBox1.Items.Clear();//清除名單
                        string[] M = Str.Split(',');//拆解名單成陣列
                        for (int i = 0; i < M.Length; i++) listBox1.Items.Add(M[i]);
                        break;
                    case "5":
                        DialogResult result = MessageBox.Show("是否重玩遊戲", "重玩訊息", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
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
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("抱歉" + listBox1.SelectedItem.ToString() + "拒絕你的邀請");
                        }
                        break;
                    case "D":
                        txtResult.Text = Str;
                        button1.Enabled = true;
                        T.Close();
                        Th.Abort();
                        break;
                    case "I":
                        string[] F = Str.Split(',');
                        DialogResult res = MessageBox.Show(F[0] + "邀請玩遊戲，是否接受?", "邀請訊息", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            int i = listBox1.Items.IndexOf(F[0]);
                            listBox1.SetSelected(i, true);
                            listBox1.Enabled = false;

                            button2.Enabled = true;
                            Send("R" + "Y" + "|" + F[0]);
                        }
                        else
                        {
                            Send("R" + "N" + "|" + F[0]);
                        }
                        break;
                    case "R":
                        if (Str == "Y")
                        {
                            MessageBox.Show(listBox1.SelectedItem.ToString() + "接受你的邀請，可以開始遊戲");
                            listBox1.Enabled = false;
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("抱歉" + listBox1.SelectedItem.ToString() + "拒絕你的邀請");
                        }
                        break;
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem.ToString() != User) //有選擇對手
                {
                    //發送邀請給選擇的對手
                    Send("I" + User + "," + "|" + listBox1.SelectedItem);
                }
                else
                {
                    MessageBox.Show("不可以邀請自己!");
                }
            }
            else
            {
                MessageBox.Show("沒有選取邀請的對象!");
            }
        }
        string my;
        bool Turn = true;
    }
    }

