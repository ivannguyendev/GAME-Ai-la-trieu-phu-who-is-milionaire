/*
 * Created by SharpDevelop.
 * User: chepchip
 * Date: 11/25/2016
 * Time: 2:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using socketmodel;
using System.Threading;
using System.Net.Sockets;
using SQL;

namespace ailatrieuphu

{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
       
        private roominfo[,] infotable;
        int waittimer = 150;
		void MainFormLoad(object sender, EventArgs e)
		{
			this.Text = "ServerUngvien";
            timer1.Tick += new EventHandler(timecounter);
			CheckForIllegalCrossThreadCalls = false;
            
            infotable = new roominfo[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    infotable[i, j] = new roominfo();
		}		
		void Button1Click(object sender, EventArgs e)
		{
			//1. Start server
			StartServer();
			//2. Receive n from client
			//3. Calculate
			//4. Send result to client						
			Thread t = new Thread(ServeClients);
			t.Start();
		}
		//cau truc du lieu va thuat toan can su dung cho viec bat server
		private TCPModel tcp;
        
		public void StartServer(){
			string ip = textBox1.Text;
			int port = int.Parse(textBox2.Text);
			tcp = new TCPModel(ip,port);
			tcp.Listen();
			button1.Enabled = false;
		}		
		
		//cau truc du lieu va thuat toan can cho viec chap nhan ket noi va giao tiep
		private SocketModel[,] socketList;
		private int numberOfClients = 10;
		private int currentClientx = 0;
        private int currentClienty = 0;
        
       
		public void ServeClients(){
            socketList = new SocketModel[numberOfClients, numberOfClients];     
			//1 server tren nguyen tac la phuc vu so clients ko gioi han
			while (true){
				ServeAClient();
			}
		}
		//ham phuc vu 1 client
		public void ServeAClient(){
            int[,] num = {{-1},{-1}};
			Accept();//chap nhap ket noi
            if (currentClientx < 10) currentClientx++;//khi co client moi ket noi, so client se tang len
            else 
                if (currentClienty < 10)
                {
                    currentClienty++;
                    currentClientx = 0;
                }
            num[0, 0] = currentClientx - 1;//do so hieu mang bat dau tu 0 	
            num[1, 0] = currentClienty;
			//do qua trinh giao tiep giua 1 client va server thong thuong 
			//ko co gioi han ve thoi gian, cho nen ta phai dat ham Communication
			//trong thread vi neu ko:
			//[1] ham Communication se lam tang thoi gian chay cho ham ServeAClient
			//[2] Neu ham ServeAClient chay cham, ham ServeClients se bi cham		
			//. Ket qua la, cac client moi ket noi vao he thong se bi cho
         
                Thread t = new Thread(Communication);
                t.Start(num);
        
		}	
		//ham chap nhan ket noi tu client
		public void Accept(){
			int status = -1;
			Socket s = tcp.SetUpANewConnection(ref status);
            socketList[currentClientx, currentClienty] = new SocketModel(s);
			//lay dia chi IP cua client va them vao textbox 3 ben phia server de quan ly ket noi
            string str = socketList[currentClientx, currentClienty].GetRemoteEndpoint();
			string str1 = "New connection from: "+ str + "\n";
			textBox3.AppendText(str1);		
		}	
		//ham giao tiep 
		public void Communication(object obj){
            int[,] pos = (Int32[,])obj;
            int count = 0;
            question cauhoi = new question();
			//xay dung vong lap vo han o day vi chung ta ko biet client muon DÙNG nhieu lan
			while (true){
                ctrl_thamgia ctrlthamgia = new ctrl_thamgia();
				string str = socketList[pos[0,0], pos[1,0]].ReceiveData();
                if (!str.Contains("Error"))
                {
                    string[] str_split = str.Split(';');
                    if (str_split[0] == "waiting")
                    {
                        string t = str_split[1];
                        if (!checkmatch(t))
                        {
                            t = ctrlthamgia.getinfor(t);
                            infotable[pos[0, 0], pos[1, 0]].Hovaten = t;
                            infotable[pos[0, 0], pos[1, 0]].Account = str_split[1];
                            BroadcastResult(maxroom(pos[1, 0]), "hovaten;" + t);
                        
                        }
                        else
                        {
                            socketList[pos[0, 0], pos[1, 0]].SendData("exited");
                            socketList[pos[0, 0], pos[1, 0]].CloseSocket();
                            return;
                        }
                    }
                    if(str_split[0] == "ready")
                    {
                        Console.WriteLine(str_split[1] + "san sang");
                        infotable[pos[0, 0], pos[1, 0]].Status = true;
                        Thread t = new Thread(checkstatus);
                        t.Start(pos[1, 0]);
                    }

                    if (str_split[0] == "aws")
                    {
                        this.Invoke((MethodInvoker)delegate
                            {
                                infotable[pos[0, 0], pos[1, 0]].Time = waittimer;
                                Console.WriteLine(infotable[pos[0, 0], pos[1, 0]].Account + ";" + str_split[1]);
                                Console.WriteLine(infotable[pos[0, 0], pos[1, 0]].Account + infotable[pos[0, 0], pos[1, 0]].Time);
                            });
                        if (str_split[1] == quesuv.Dapandung) infotable[pos[0, 0], pos[1, 0]].Dung = true;
                    }
                    else if (str_split[0] == "awsmp")
                    {
                        if (str_split[1] == cauhoi.Dapandung) BroadcastResult(maxroom(pos[1, 0]), "awsmp;correct;" + infotable[pos[0, 0], pos[1, 0]].Account);
                        else BroadcastResult(maxroom(pos[1, 0]), "awsmp;faile");
                    }
                
                    if (str_split[0] == "next")
                    {
                        cauhoi = ctrques.getques_mainplayer(++count);
                        Sendques_mainplayer(cauhoi, count, pos[1, 0]);   
                    }
                    else if (str_split[0] == "start")
                    {
                        ctrques.loadques_mainplayer();
                        cauhoi = ctrques.getques_mainplayer(++count);
                        Sendques_mainplayer(cauhoi, count, pos[1, 0]);   
                    }
                    if (str_split[0] == "c5050") BroadcastResult(maxroom(pos[1, 0]), "c5050;" + c5050(Convert.ToInt32(cauhoi.Dapandung[0]) - Convert.ToInt32('A') + 1));
                    else if (str_split[0] == "hoikhangia") BroadcastResult(maxroom(pos[1, 0]), "hoikhangia;" + KhanGia(count, cauhoi.Dapandung));

                }
                else
                {
                    currentClientx--;
                    socketList[pos[0, 0], pos[1, 0]].CloseSocket();
                    infotable[pos[0, 0], pos[1, 0]].Hovaten = infotable[pos[0, 0], pos[1, 0]].Account = "";
                    infotable[pos[0, 0], pos[1, 0]].Dung = infotable[pos[0, 0], pos[1, 0]].Status = false;
                    infotable[pos[0, 0], pos[1, 0]].Time = 0;
                }     
			}
		}
        private string c5050(int dapan)
        {  
                string s = "";
                int tmp;
                //random mot dap an
                Random rnd = new Random();
                //list cac dap an con lai
                List<int> list = new List<int>();
                for (int i = 1; i < 4; i++)
                {
                    if (dapan != i)
                        list.Add(i);
                }
                //random cac dap an sai
                for (int i = 0; i < 2; i++)
                {
                    tmp = rnd.Next(list.Count);
                    s += list[tmp];
                    list.RemoveAt(tmp);
                }
                return s;
        }
        private string KhanGia(int pos, string dapan)
        {
            int t = Convert.ToInt32(dapan[0]) - Convert.ToInt32('A') + 1;
            //random % ket qua
            int[] A = new int[4];
            Random rnd = new Random();
            for (int i=0; i<4 ; i++)
                if (t == i) A[i] = rnd.Next(0, 100);
                else A[i] = rnd.Next(0, 80);

            int tong = A[0] + A[1] + A[2] + A[3];
            return Math.Round((float)A[0] / tong, 2) + ";" + Math.Round((float)A[1] / tong, 2) + ";" + Math.Round((float)A[2] / tong, 2) + ";" + Math.Round((float)A[3] / tong, 2);
        }
        private bool checkmatch(string account)
        {
            for(int i = 0; i < 10  ; i++)
                for(int j = 0; i < 10 && infotable[i,j].Account != ""; j++)
                if (account == infotable[i,j].Account) return true;
            return false;
        }
        private void Sendques_mainplayer(question cauhoi, int count, int y)
        {
           
            for (int i = 3; i > 0; i--)
            {
                BroadcastResult(maxroom(y), "countmp;" + i);
                Thread.Sleep(1000);
            }
            BroadcastResult(maxroom(y), "ndmp;"+ count +";Câu " + count + ".  " + cauhoi.Noidung.Trim());
            BroadcastResult(maxroom(y), "dAmp;" + cauhoi.Dapan1.Trim() + ";" + cauhoi.Dapan2.Trim() + ";" + cauhoi.Dapan3.Trim() + ";" + cauhoi.Dapan4.Trim());
        }
        public void BroadcastResult(int max, string result)
        {
            result = result.Trim();
            for (int i = 0; i < max; i++)
            {
               socketList[i, currentClienty].SendData(result);
            }
        }
        //Biến dữ liệu SQL
        question_uv quesuv = new question_uv();
        ctrl_ques ctrques = new ctrl_ques();
        public void checkstatus(object obj)
        {
            int max = 0, ready = 0;
            int y = (Int32)obj;
            max = maxroom(y);
            for (int i = 0; infotable[i, y].Status == true ; i++) ready++;
            if (ready == max)
            {
                
                this.Invoke((MethodInvoker)delegate
                {
                    timer1.Tag = y;
                    timer1.Enabled = true;
                    timer1.Start();
                });
                
            }

        }
        private int maxroom(int y)
        {
            int max = 0;
            for (int i = 0; infotable[i, y].Account != ""; i++) max++;
            return max;
        }
        private string findmainplayer(int y)
        {
            int mainplayer = 0;
            for(int i = 0; i <10 ; i++)
            {
                if (infotable[i, y].Dung && infotable[i, y].Time > infotable[mainplayer, y].Time)
                    mainplayer = i;
            }
            if (mainplayer == 0 && !infotable[0, y].Dung)
                return "-1";
            return infotable[mainplayer, y].Account + ";"+infotable[mainplayer, y].Hovaten;
        }
        private void timecounter (object sender, EventArgs e)
        {
            if(waittimer == 150)
            {
                int y = (Int32)timer1.Tag;
                int max = maxroom(y);
                for (int i = 3; i > 0; i--)
                {
                    BroadcastResult(maxroom(y), "count;" + i);
                    Thread.Sleep(1000);
                }
                BroadcastResult(max, "count;" + "BẮT ĐẦU");

                quesuv = ctrques.getques_player();
                quesuv.Dapan1 = quesuv.Dapan1.Trim();
                quesuv.Dapan2 = quesuv.Dapan2.Trim();
                quesuv.Dapan3 = quesuv.Dapan3.Trim();
                quesuv.Dapan4 = quesuv.Dapan4.Trim();
                quesuv.Dapan5 = quesuv.Dapan5.Trim();
                quesuv.Dapan6 = quesuv.Dapan6.Trim();
                quesuv.Dapandung = quesuv.Dapandung.Trim();
                quesuv.Noidung = quesuv.Noidung.Trim();
                quesuv.Id = quesuv.Id.Trim();
                Console.WriteLine(quesuv.Dapandung);

                BroadcastResult(maxroom(y), "nd;" + quesuv.Noidung);
                BroadcastResult(maxroom(y), "dA;" + quesuv.Dapan1 + ";" + quesuv.Dapan2 + ";" + quesuv.Dapan3 + ";" + quesuv.Dapan4 + ";" + quesuv.Dapan5 + ";" + quesuv.Dapan6);
            }
            if (waittimer > 0)
            {
                waittimer -= 1;
                //Console.WriteLine(waittimer);
            }
            else
            {
                int y = (Int32)timer1.Tag;
                string str = findmainplayer(y);
                if (str != "-1")
                {
                    BroadcastResult(maxroom(y), "mainplayer;" + str);
                    waittimer = 150;
                    timer1.Stop();
                    return;
                }
                waittimer = 150;
                
            }
               
        }
        
		
	}
}
