using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Client
{
    public partial class Form1 : Form
    {
        private TCPModel tcpForClient;
        private TCPModel tcpForOpponent;
        
        private int port = 12000;
        private TCPModel tcpForChoosingServer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            //1. Connect to server	
            DetectServer();
            Connect(port);
        }
        public void Connect(int port)
        {
            string ip = "127.0.0.1";
            //port = int.Parse(textBox2.Text);
            tcpForClient = new TCPModel(ip, port);
            tcpForClient.ConnectToServer();
            tcpForOpponent = new TCPModel(ip, port);
            tcpForOpponent.ConnectToServer();
            //tcpForClient2 = new TCPModel(ip, port);
            //tcpForClient2.ConnectToServer();
            //tcpForOpponent2 = new TCPModel(ip, port);
            //tcpForOpponent2.ConnectToServer();

            Thread t = new Thread(Receiver);
            t.Start();
            // nguoiChoi = tcpForClient.ReadData();
            //  Console.WriteLine("Nguoi choi la: " + nguoiChoi);
        }
        public void DetectServer()
        {
            string ip = "127.0.0.1";
            tcpForChoosingServer = new TCPModel(ip, 12000);
            tcpForChoosingServer.ConnectToServer();
            tcpForChoosingServer.SendData("login");
            string str = tcpForChoosingServer.ReadData();
            port = int.Parse(str);
            Console.WriteLine("Port la: " + port);
        }
        void Receiver()
        {
            while (true)
            {
                //3. Receive result of n! from server			
                string result = tcpForClient.ReadData();
                // Check exist of server
                if (!result.Contains("Error"))
                {
                    if (result.Contains("Success"))
                    {
                        btn_dangnhap.Enabled = true;
                        Console.WriteLine("dang nhap thanh cong");
                        
                        for (int i = 0; i <= 10; i++)
                        {
                            Thread.Sleep(20);
                            this.Opacity = i * 0.1;
                        }
                        Form frm = new Form3(txt_tendangnhap.Text);                 // Tạo form 
                        frm.ShowDialog();
                        this.ShowInTaskbar = true;              // Hiện icon ở thanh taskbar
                    }
                    else
                    {
                        btn_dangnhap.Enabled = true;
                        Console.WriteLine("dang nhap that bai");
                        MessageBox.Show("Đăng nhập thất bại", "Đăng nhập thất bại", MessageBoxButtons.OK);
                    }
                }
                else
                    {
                        DetectServer();
                        //if (port == 14000) Connect(13000);
                        //else
                        //    if(port == 13000) Connect(14000);
                        Connect(port);
                    }
            }

        }
        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            //2. Send n to server
            string n = txt_tendangnhap.Text + ":" + txt_password.Text;
            Console.WriteLine(txt_tendangnhap.Text + txt_password.Text);
            tcpForClient.SendData(n);
            btn_dangnhap.Enabled = false;
            txt_password.Clear();
        }

        private void btn_taotaikhoan_Click(object sender, EventArgs e)
        {
            Form frm = new Form2();                
            frm.ShowDialog();
        }
    }
}
