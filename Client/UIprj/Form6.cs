using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form6 : Form
    {
        string q, da1, da2, da3, da4;
        string account = "";
        public Form6()
        {
            InitializeComponent();
        }
        public Form6(string ac, string s, string s1, string s2, string s3, string s4)
        {
            InitializeComponent();
            ac = account;
            q = s;
            da1 = s1;
            da2 = s2; 
            da3 = s3;
            da4 = s4;
        }
        
        private TCPModel tcpForClient;

        private int port = 12000;
        private TCPModel tcpForChoosingServer;
        int count = 0;
        

        private void Form6_Load(object sender, EventArgs e)
        {
            //1. Connect to server	
            DetectServer();
            Connect(port);
            //2. Send n to server
            string n = "waiting;" + account;
            Console.WriteLine(n);
            tcpForClient.SendData(n);
        }
        public void Connect(int port)
        {
            string ip = "127.0.0.1";
            //port = int.Parse(textBox2.Text);
            tcpForClient = new TCPModel(ip, port);
            tcpForClient.ConnectToServer();

            Thread t = new Thread(Receiver);
            t.Start();
        }
        public void DetectServer()
        {
            string ip = "127.0.0.1";
            tcpForChoosingServer = new TCPModel(ip, 12000);
            tcpForChoosingServer.ConnectToServer();
            tcpForChoosingServer.SendData("uv");
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
                    //Method update GUI
                    this.Invoke((MethodInvoker)delegate
                    {
                        richTextBox2.AppendText(result);
                    });
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "") tcpForClient.SendData(richTextBox1.Text);
        }
        private void btn_start_Click(object sender, EventArgs e)
        {

        }
    }
}
