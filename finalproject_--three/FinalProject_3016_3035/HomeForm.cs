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
using System.Threading;
using FinalProject_3016_3035;//匯入多執行緒功能函數

namespace FinalProject
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
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
            button2.Enabled = false; // 禁用「開始遊戲」按鈕        }
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
                Send("0" + User); // 發送用戶名稱到伺服器
                button1.Enabled = false; // 禁用「連線伺服器」按鈕
                button2.Enabled = false;
                button3.Enabled = true;

            }
            catch
            {
                textBox4.Text = "無法連線伺服器!" + "\r\n"; // 顯示連線失敗訊息
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
                    case "D":
                        textBox4.Text = Str;
                        button1.Enabled = true;
                        T.Close();
                        Th.Abort();
                        break;
                    case "I":
                        string[] F = Str.Split(',');
                        DialogResult res = MessageBox.Show(F[0] + "邀請玩遊戲(" + F[1] + ")，是否接受?", "邀請訊息", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {                           
                            int i = listBox1.Items.IndexOf(F[0]);
                            listBox1.SetSelected(i, true);
                            listBox1.Enabled = false;
                            comboBox1.Text = F[1];
                            comboBox1.Enabled = false;
                            button2.Enabled = true;
                            button3.Enabled = false;
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
                            button3.Enabled = false;
                            button2.Enabled = true;
                            comboBox1.Enabled = false;
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

            if (comboBox1.Text == "猜拳遊戲")
            {
                // 剩餘的資料
                string text1 = textBox1.Text;
                string text2 = textBox2.Text;
                string text3 = textBox3.Text;
                var listData = listBox1.Items.Cast<string>().ToList();

                // 傳入參數建立 gameForm
                GameForm gameForm = new GameForm(playerName, opponentName, text1, text2, text3, listData);

                // 顯示表單
                gameForm.Show();
            }
            else if (comboBox1.Text == "飛碟球遊戲")
            {
                string text1 = textBox1.Text;
                string text2 = textBox2.Text;
                string text3 = textBox3.Text;
                var listData = listBox1.Items.Cast<string>().ToList();

                // 創建 InviteFrom 並傳遞當前的 Form1
                InviteFrom inviteFrom = new InviteFrom(playerName, opponentName, text1, text2, text3, listData);
                inviteFrom.Show(); // 顯示 InviteFrom
                this.Hide();     // 隱藏 Form1
            }
            else
            {
                MessageBox.Show("沒有選取遊戲!");
            }         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (listBox1.SelectedItem.ToString() != User) //有選擇對手
                {
                    //發送邀請給選擇的對手
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
    }
}