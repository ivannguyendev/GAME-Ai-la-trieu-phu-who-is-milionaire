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
			this.Text = "ROOTSV";	
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
        private Object thisLock = new Object();
		public void ServeClients(){
			socketList = new SocketModel[numberOfClients];
            
			//1 server tren nguyen tac la phuc vu so clients ko gioi han
			while (true){
				ServeAClient();
			}
		}
        //ham giao tiep 
        public void Communication(int num)
        {
            string str = socketList[num].ReceiveData();
            if (str == "login") str = "13000";
            else if (str == "ml") str = "15000";
            else if (str == "uv") str = "14000";
            socketList[num].SendData(str);		
        }	
      
		//ham phuc vu 1 client
		public void ServeAClient(){
			int num = -1;
            lock (thisLock)
            {
                Accept();
                currentClient++;
                num = currentClient - 1;	
            }
            Communication(num);

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
				
	}
}
