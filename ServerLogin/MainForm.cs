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
		
		void MainFormLoad(object sender, EventArgs e)
		{
			this.Text = "ServerLogin";	
			CheckForIllegalCrossThreadCalls = false;	
		
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
        ctrl_thamgia ctrlthamgia = new ctrl_thamgia();
		public void StartServer(){
			string ip = textBox1.Text;
			int port = int.Parse(textBox2.Text);
			tcp = new TCPModel(ip,port);
			tcp.Listen();
			button1.Enabled = false;
		}		
		
		//cau truc du lieu va thuat toan can cho viec chap nhan ket noi va giao tiep
		private SocketModel[] socketList;
		private int numberOfClients = 100;	
		private int currentClient = 0;
       
		public void ServeClients(){
			socketList = new SocketModel[numberOfClients];
            
			//1 server tren nguyen tac la phuc vu so clients ko gioi han
			while (true){
				ServeAClient();
			}
		}
		//ham phuc vu 1 client
		public void ServeAClient(){
			int num = -1;
			Accept();//chap nhap ket noi
			currentClient ++;//khi co client moi ket noi, so client se tang len	
			num = currentClient-1;//do so hieu mang bat dau tu 0 	
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
			socketList[currentClient] = new SocketModel(s);
			//lay dia chi IP cua client va them vao textbox 3 ben phia server de quan ly ket noi
			string str = socketList[currentClient].GetRemoteEndpoint();
			string str1 = "New connection from: "+ str + "\n";
			textBox3.AppendText(str1);		
		}	
		//ham giao tiep 
       
		public void Communication(object obj){
			int pos = (Int32) obj;
			//xay dung vong lap vo han o day vi chung ta ko biet client muon DÙNG nhieu lan
			while (true){
				string str = socketList[pos].ReceiveData();
                string[] str_split = str.Split(':');
                Console.WriteLine(str_split[0] + str_split[1]);
                if (ctrlthamgia.login(str_split[0], str_split[1]))
                {
                    socketList[pos].SendData("Success");
                }   
                else socketList[pos].SendData("Faile");

                 
			}
		}
		
		
	}
}
