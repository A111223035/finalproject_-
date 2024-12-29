using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace FinalProject_3016_3035
{
    public partial class InviteFrom : Form
    {
        public InviteFrom()
        {
            InitializeComponent();           
        }

        public InviteFrom(string playerName, string opponentName)
        {
            this.playerName = playerName;
            this.opponentName = opponentName;
        }

        // 宣告變數
        Socket T; // 用於網絡通信的 Socket
        Thread Th; // 用於接收數據的執行緒
        string User; // 儲存用戶名稱
//        Color original; // 儲存按鈕的初始背景顏色
        int mdx; // 拖曳球拍起點
        int oX = 0; // 球拍位置

        int playerScore = 0; // 玩家分數
        int opponentScore = 0; // 對手分數
        int speedIncreaseCount = 0; // 用來增加球速的計數器

        // 獲取本地 IP 地址
        private string MyIP()
        {
            string hn = Dns.GetHostName(); // 獲取主機名稱
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList; // 獲取所有 IP 地址
            foreach (IPAddress it in ip)
            {
                if (it.AddressFamily == AddressFamily.InterNetwork) // 過濾 IPv4 地址
                {
                    return it.ToString(); // 返回第一個 IPv4 地址
                }
            }
            return ""; // 如果沒有找到，返回空字符串
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += MyIP() + " - " + User; ; // 在窗體標題中顯示本地 IP 地址
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
        }

        // 發送數據到伺服器
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str); // 將字符串編碼為字節數組
            T.Send(B, 0, B.Length, SocketFlags.None); // 通過 Socket 發送數據
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem.ToString() != User)//有選擇對手
                {
                    Send("I" + User + "," +  "|" + listBox1.SelectedItem);
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
        string my;
        bool Turn = true;
        private string playerName;
        private string opponentName;

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
                    T.Close();
                    listBox1.Items.Clear();
                    MessageBox.Show("伺服器斷線了!");
                    button1.Enabled = true;
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
                        DialogResult result = MessageBox.Show("是否重玩遊戲(" + Str + "輪)?", "重玩訊息", MessageBoxButtons.YesNo);
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

                    case "D":
                        textBox4.Text = Str;
                        button3.Enabled = true;
                        button2.Enabled = false;
                        T.Close();
                        Th.Abort();
                        break;
                    case "I":
                        string[] F = Str.Split(',');
                        DialogResult res = MessageBox.Show(F[0] + "邀請玩遊戲(" + F[1] + "輪)，是否接受?", "邀請訊息", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            int i = listBox1.Items.IndexOf(F[0]);
                            listBox1.SetSelected(i, true);
                            listBox1.Enabled = false;
//                            comboBox1.Text = F[1];
//                            comboBox1.Enabled = false;
                            button3.Enabled = false;
                            button1.Enabled = true;
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
//                            comboBox1.Enabled = false;
                            button3.Enabled = false;
                            button1.Enabled = true;
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("抱歉" + listBox1.SelectedItem.ToString() + "拒絕你的邀請");
                        }
                        break;

                    case "7":
                        H2.Left = G.Width - int.Parse(Str) - H2.Width;
                        break;
                    case "8":
                        string[] C = Str.Split(',');
                        Q.Left = G.Width - int.Parse(C[0]) - Q.Width;
                        Q.Top = G.Height - Q.Height - int.Parse(C[1]);
                        break;
                }
            }
        }           
           

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (button1.Enabled == false)
            {
                Send("9" + User);// 在關閉表單前，通知伺服器用戶已離線。
                T.Close();// 關閉套接字連線。
            }
        }      


        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                Send("5" + "|" + listBox1.SelectedItem);
            }
            playerScore = 0;
            opponentScore = 0;
            UpdateScore();
            ResetBall();
        }

        // 啟動遊戲
        private void button2_Click(object sender, EventArgs e)
        {
//            Send("L");
            Q.Tag = new Point(5, -5); // 預設速度(往右上)
            Timer1.Start(); // 開始移動
        }

        // 開始拖曳球拍
        private void H1_MouseDown(object sender, MouseEventArgs e)
        {
            mdx = e.X;
        }

        // 拖曳球拍中
        private void H1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int X = H1.Left + e.X - mdx;
                if (X < 0) X = 0;
                if (X > G.Width - H1.Width) X = G.Width - H1.Width;
                H1.Left = X;
                if (listBox1.SelectedIndex >= 0)
                {
                    if (oX != H1.Left)
                    {
                        Send("7" + H1.Left.ToString() + "|" + listBox1.SelectedItem);
                        oX = H1.Left;
                    }
                }
            }
        }

        // 控制球移動的程式
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Point V = (Point)Q.Tag;
            Q.Left += V.X;
            Q.Top += V.Y;
            chkHit(Q, G, true);
            chkHit(Q, H1, false);
            chkHit(Q, H2, false);

            // 如果球進入對方區域得分
            if (Q.Top > G.Height)
            {
                opponentScore++;
                UpdateScore(); // 更新分數顯示
                ResetBall();   // 重置球的位置
            }
            else if (Q.Top < 0)
            {
                playerScore++;
                UpdateScore(); // 更新分數顯示
                ResetBall();   // 重置球的位置
            }

            // 逐漸增加球速
            if (speedIncreaseCount >= 2) // 每2次增加球速
            {
                IncreaseSpeed(); // 增加球速
                speedIncreaseCount = 0; // 重置計數器
            }
            else
            {
                speedIncreaseCount++; // 增加計數器
            }

            // 強制更新畫面
            this.Invalidate(); // 重新繪製畫面

            if (listBox1.SelectedIndex >= 0)
            {
                Send("8" + Q.Left.ToString() + "," + Q.Top.ToString() + "|" + listBox1.SelectedItem);
            }
        }

        // 碰撞檢查程式
        private bool chkHit(Label B, object C, bool inside)
        {
            Point V = (Point)B.Tag;
            if (inside)
            {
                Panel p = (Panel)C;
                if (B.Right > p.Width)
                {
                    V.X = -Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                if (B.Left < 0)
                {
                    V.X = Math.Abs(V.X);
                    B.Tag = V;
                    return true;
                }
                if (B.Bottom > p.Height)
                {
                    V.Y = -Math.Abs(V.Y);
                    B.Tag = V;
                    return true;
                }
                if (B.Top < 0)
                {
                    V.Y = Math.Abs(V.Y);
                    B.Tag = V;
                    return true;
                }
                return false;
            }
            else
            {
                Label k = (Label)C;
                if (B.Right < k.Left || B.Left > k.Right || B.Bottom < k.Top || B.Top > k.Bottom)
                    return false;

                // 處理左右碰撞
                if (B.Right >= k.Left && (B.Right - k.Left) <= Math.Abs(V.X)) V.X = -Math.Abs(V.X);
                if (B.Left <= k.Right && (k.Right - B.Left) <= Math.Abs(V.X)) V.X = Math.Abs(V.X);

                // 處理上下碰撞
                if (B.Top <= k.Bottom && (k.Bottom - B.Top) <= Math.Abs(V.Y)) V.Y = Math.Abs(V.Y);
                if (B.Bottom >= k.Top && (B.Bottom - k.Top) <= Math.Abs(V.Y)) V.Y = -Math.Abs(V.Y);

                B.Tag = V;
                return true;
            }
        }

        // 更新分數顯示
        private void UpdateScore()
        {
            lblPlayerScore.Text = "我方得分： " + playerScore.ToString();
            lblOpponentScore.Text = "對手得分： " + opponentScore.ToString();
            if (playerScore >= 3)
            {
                MessageBox.Show("You Win!");
                ResetGame();
            }
            else if (opponentScore >= 3)
            {
                MessageBox.Show("Opponent Wins!");
                ResetGame();
            }
        }
        // 重置遊戲
        private void ResetGame()
        {
            playerScore = 0;
            opponentScore = 0;
            UpdateScore();
            ResetBall();
        }

        // 重置球的位置
        private void ResetBall()
        {
            Q.Left = G.Width / 2 - Q.Width / 2;
            Q.Top = G.Height / 2 - Q.Height / 2;
            Q.Tag = new Point(5, -5); // 重置球的速度
        }

        // 增加球速
        private void IncreaseSpeed()
        {
            Point currentSpeed = (Point)Q.Tag;
            currentSpeed.X = (int)(currentSpeed.X * 1.1); // 逐漸增加X軸速度
            currentSpeed.Y = (int)(currentSpeed.Y * 1.1); // 逐漸增加Y軸速度
            Q.Tag = currentSpeed;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();  // 退出遊戲
        }
    }
}
