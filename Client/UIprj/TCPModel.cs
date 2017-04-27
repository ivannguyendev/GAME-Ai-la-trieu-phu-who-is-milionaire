/*
 * Created by SharpDevelop.
 * User: chepchip
 * Date: 11/11/2016
 * Time: 12:35 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Client
{
	/// <summary>
	/// Description of TCPModel.
	/// </summary>
	public class TCPModel
	{
		private TcpClient tcpclnt;	
		private Stream stm;
		private byte[] byteSend;
		private byte[] byteReceive;
		private string IPofServer;
		private int port;
		
		public TCPModel(string ip, int p)
		{
			IPofServer = ip;
			port = p;
			tcpclnt = new TcpClient();			
			byteReceive = new byte[100];
		}
		//show information of client to update UI
		public string UpdateInformation(){
			string str = "";
			try{
				Socket s = tcpclnt.Client;
				str = str + s.LocalEndPoint;
				Console.WriteLine(str);							
			}
			catch (Exception e){
				Console.WriteLine("Error..... " + e.StackTrace);
			}
			return str;
		}
		//Set up a connection to server and get stream for communication
		public int ConnectToServer(){			
			try{
				tcpclnt.Connect(IPofServer, port);
				stm = tcpclnt.GetStream();
				Console.WriteLine("OK!");
			}
			catch (Exception e){
	            Console.WriteLine("Error..... " + e.StackTrace);
				return -1;	            
			}
			return 1;
		}
		//Send data to server after connection is set up
		public bool SendData(string str){
            try
            {

                byteSend = Encoding.UTF8.GetBytes(str);
                stm.Write(byteSend, 0, byteSend.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
                return false;
            }
            return true;
		}
		//Read data from server after connection is set up
		public string ReadData(){
            string str = "";
            try
            {
                //count the length of data received
                int k = stm.Read(byteReceive, 0, 100);
                Console.WriteLine("Length of data received: " + k.ToString());
                Console.WriteLine("From server: ");
                //conver the byte recevied into string

                str = Encoding.UTF8.GetString(byteReceive,0,k);
                Console.Write(str);
                
            }
            catch (Exception e)
            {
                str = "Error..... " + e.StackTrace;
                Console.WriteLine(str);
            }
            return str;
		}
		//close connection
		public void CloseConnection(){
            stm.Close();
			tcpclnt.Close();
            
		}
		
	}
}
