using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;//匯入網路通訊協定相關參數
using System.Net.Sockets;//匯入網路插座功能函數
using System.Threading;//匯入多執行緒功能函數

namespace FinalProject
{
    public partial class 歡迎來到林珈羽和黃香綾製作的遊戲 : Form
    {
        public 歡迎來到林珈羽和黃香綾製作的遊戲()
        {
            InitializeComponent();
            button2.Enabled = false;
        }
        // 宣告變數
        Socket T; // 用於網絡通信的 Socket
        Thread Th; // 用於接收數據的執行緒
        string User; // 儲存用戶名稱

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

        // 發送數據到伺服器
        private void Send(string Str)
        {
            byte[] B = Encoding.Default.GetBytes(Str); // 將字符串編碼為字節數組
            T.Send(B, 0, B.Length, SocketFlags.None); // 通過 Socket 發送數據
        }

        // 窗體加載事件
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += MyIP(); // 在窗體標題中顯示本地 IP 地址
            button2.Enabled = false; // 禁用「開始遊戲」按鈕
            button3.Enabled = false; // 禁用「邀請玩家」按鈕
        }

        // 連線伺服器按鈕點擊事件
        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; // 禁用執行緒安全檢查
            User = textBox3.Text; // 獲取用戶名稱
            string IP = textBox1.Text; // 獲取伺服器 IP 地址
            int Port = int.Parse(textBox2.Text); // 獲取伺服器端口號
            try
            {
                IPEndPoint EP = new IPEndPoint(IPAddress.Parse(IP), Port); // 創建終端點
                T = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // 創建 TCP 套接字
                T.Connect(EP); // 連接到伺服器
                Th = new Thread(Listen); // 創建執行緒處理伺服器消息
                Th.IsBackground = true; // 設定為背景執行緒
                Th.Start(); // 啟動執行緒
                textBox4.Text = "已連線伺服器!" + "\r\n"; // 顯示連線成功訊息
                button2.Enabled = false;
                Send("0" + User); // 發送用戶名稱到伺服器
                button1.Enabled = false; // 禁用「連線伺服器」按鈕
                button3.Enabled = true; // 啟用「邀請玩家」按鈕
            }
            catch
            {
                textBox4.Text = "無法連線伺服器!" + "\r\n"; // 顯示連線失敗訊息
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem.ToString() != User)
                {
                    Send("I" + User + "," + comboBox1.Text + "|" + listBox1.SelectedItem);
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
                        DialogResult result = MessageBox.Show("是否重玩遊戲(連" + Str + "排)?", "重玩訊息", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            comboBox1.Text = Str;
                            comboBox1.Enabled = false;
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
                        button1.Enabled = true;
                        button3.Enabled = false;
                        T.Close();
                        Th.Abort();
                        break;
                    case "I":
                        string[] F = Str.Split(',');
                        DialogResult res = MessageBox.Show(F[0] + "邀請玩遊戲(連" + F[1] + "排)，是否接受?", "邀請訊息", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            int i = listBox1.Items.IndexOf(F[0]);
                            listBox1.SetSelected(i, true);
                            listBox1.Enabled = false;
                            comboBox1.Text = F[1];
                            comboBox1.Enabled = false;
                            button3.Enabled = false;
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
                            comboBox1.Enabled = false;
                            button3.Enabled = false;
                            button2.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("抱歉" + listBox1.SelectedItem.ToString() + "拒絕你的邀請");
                        }
                        break;
                    case "3":
                        textBox4.Text = "使用者名稱重複，請重新輸入";
                        button1.Enabled = true;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        T.Close();
                        Th.Abort();
                        break;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (button1.Enabled == false)
            {
                Send("9" + User);  // 在關閉表單前，通知伺服器用戶已離線。
                T.Close();        // 關閉套接字連線。
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // 定義玩家和對手名稱
            string playerName = "玩家1";
            string opponentName = "玩家2";

            // 創建 GameForm 並傳遞當前的 Form1
            GameForm gameForm = new GameForm(playerName, opponentName);
            gameForm.Show(); // 顯示 GameForm
            this.Hide();     // 隱藏 Form1
        }
    }
}