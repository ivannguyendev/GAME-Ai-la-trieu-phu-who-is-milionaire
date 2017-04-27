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
    public partial class Form3 : Form
    {
        string account = "";
        private TCPModel tcpForClient;

        private int port = 12000;
        private TCPModel tcpForChoosingServer;
        int count = 0;
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string str)
        {
            InitializeComponent();
            account = str;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            pnl_screenmainplayer.Hide();
            pnl_quesmainplayer.Hide();
            pnl_chonnguoichoi.Dock = DockStyle.Left;
            CheckForIllegalCrossThreadCalls = false;
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
                {
                    if (result.Contains("hovaten;"))
                    {
                        string[] str_split = result.Split(';');
                        this.Invoke((MethodInvoker)delegate
                        {
                            int rowId = dgv_nguoithamgia.Rows.Add();
                            DataGridViewRow row = dgv_nguoithamgia.Rows[rowId];
                            row.Cells["hovaten"].Value = str_split[1].Trim();
                        });
                    }
                    else if (result == "exited")
                    {
                        MessageBox.Show("Tài khoản đang được sử dụng!!!", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tcpForClient.CloseConnection();
                        this.Close();       
                    }

                    if (result.Contains("nd;"))
                    {
                        btn_ready.Enabled = true;
                        btn_ready.Hide();
                        string[] str_split = result.Split(';');
                        this.Invoke((MethodInvoker)delegate
                        {
                            lbl_ques_choose.Text = str_split[1].Trim();
                        });
                    }
                    else if(result.Contains("ndmp;"))
                    {
                        string[] str_split = result.Split(';');
                        this.Invoke((MethodInvoker)delegate
                        {
                             lbl_ques.Text = str_split[2].Trim();
                             count = (Convert.ToInt32(str_split[1]));
                             ques_number(count);
                        });
                    
                    }
                    if (result.Contains("5050;"))
                    {
                        string[] str_split = result.Split(';');
                        string str = str_split[1];
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (str[0] == '1' || str[1] == '1') { rbn_A.Enabled = false; lbl_A.Text = String.Empty; }
                            if (str[0] == '2' || str[1] == '2') { rbn_B.Enabled = false; lbl_B.Text = String.Empty; }
                            if (str[0] == '3' || str[1] == '3') { rbn_C.Enabled = false; lbl_C.Text = String.Empty; }
                            if (str[0] == '4' || str[1] == '4') { rbn_D.Enabled = false; lbl_D.Text = String.Empty; }
                        });
                    }
                    else if (result.Contains("hoikhangia;"))
                    {
                        Form frm = new Form5(result);
                        frm.ShowDialog();
                    }
                    
                    if (result.Contains("dA;"))
                    {
                        //timer1.Start();
                        string[] str_split = result.Split(';');
                        btn_aws.Enabled = true;
                        this.Invoke((MethodInvoker)delegate
                        {
                            lbl_aws_choose1.Text = "1.  " + str_split[1].Trim();
                            lbl_aws_choose2.Text = "2.  " + str_split[2].Trim();
                            lbl_aws_choose3.Text = "3.  " + str_split[3].Trim();
                            lbl_aws_choose4.Text = "4.  " + str_split[4].Trim();
                            lbl_aws_choose5.Text = "5.  " + str_split[5].Trim();
                            lbl_aws_choose6.Text = "6.  " + str_split[6].Trim();

                        });
                    }
                    else if (result.Contains("dAmp;"))
                        {
                            string[] str_split = result.Split(';');
                            this.Invoke((MethodInvoker)delegate
                            {
                                lbl_A.Text = "A.  " + str_split[1].Trim();
                                lbl_B.Text = "B.  " + str_split[2].Trim();
                                lbl_C.Text = "C.  " + str_split[3].Trim();
                                lbl_D.Text = "D.  " + str_split[4].Trim();
                            });
                        }

                    if (result.Contains("awsmp;"))
                    {
                        string[] str_split = result.Split(';');
                        
                        if (str_split[1] == "correct" )
                        {
                            if (str_split[2] == account) btn_next.Enabled = true;
                            lbl_ques.Text = "Xin chúc mừng đã vượt qua câu số" + count;
                            if (count == 15)
                            {
                                Form frm = new GameOver(diemso(count));
                                frm.ShowDialog();
                                this.Close();
                            }
                        }
                        else
                        {
                           
                            if (count<=5) count = 1;
                            else if (count <= 10) count = 5;
                            else if (count <= 15) count = 10;
                            tcpForClient.CloseConnection();

                            Form frm = new GameOver(diemso(count));
                            frm.ShowDialog();
                            this.Close();
                        }
                    }
                    else if (result.Contains("mainplayer;"))
                    {
                        string[] str_split = result.Split(';');
                        btn_start.Hide();
                        pnl_chonnguoichoi.Hide();
                        pnl_screenmainplayer.Show();
                        pnl_quesmainplayer.Show();
                        pnl_trogiup.Enabled = false;
                        btn_stop.Enabled = false;
                        btn_next.Enabled = false;
                        btn_play.Enabled = false;
                        rbn_A.Enabled = false;
                        rbn_B.Enabled = false;
                        rbn_C.Enabled = false;
                        rbn_D.Enabled = false;
                        this.Invoke((MethodInvoker)delegate
                        {
                            lbl_name.Text = "Người chơi chính: \n " + str_split[2].Trim();
                        });
                        if (account == str_split[1].Trim())
                        {
                            pnl_trogiup.Enabled = true;
                            btn_next.Enabled = false;
                            btn_start.Show();
                            btn_stop.Enabled = true;
                            btn_play.Enabled = true;
                            rbn_A.Enabled = true;
                            rbn_B.Enabled = true;
                            rbn_C.Enabled = true;
                            rbn_D.Enabled = true;
  
                        }  
                    }

                    if (result.Contains("count;"))
                    {
                        string[] str_split = result.Split(';');
                        this.Invoke((MethodInvoker)delegate
                        {
                            label9.Text = str_split[1].Trim();
                        });
                    }
                    else if (result.Contains("countmp;"))
                    {
                        string[] str_split = result.Split(';');
                        this.Invoke((MethodInvoker)delegate
                        {
                            lbl_ques.Text = str_split[1].Trim();
                            btn_play.Enabled = true;
                        });

                    }
                }
                else
                {
                    this.Close();
                    DetectServer();
                    //if (port == 14000) Connect(13000);
                    //else
                    //    if(port == 13000) Connect(14000);
                    Connect(port);
                }
                
            }

        }
        private void ques_number(int n)
        {
            switch(n)
            {
                case 1: pnl_1.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_1.ForeColor = System.Drawing.Color.White;
                        lbl_1d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 2: pnl_1.BackgroundImage = null;
                        lbl_1.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_1d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_2.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_2.ForeColor = System.Drawing.Color.White;
                        lbl_2d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 3: pnl_2.BackgroundImage = null;
                        lbl_2.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_2d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_3.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_3.ForeColor = System.Drawing.Color.White;
                        lbl_3d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 4: pnl_3.BackgroundImage = null;
                        lbl_3.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_3d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_4.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_4.ForeColor = System.Drawing.Color.White;
                        lbl_4d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 5: pnl_4.BackgroundImage = null;
                        lbl_4.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_4d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_5.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_5.ForeColor = System.Drawing.Color.White;
                        lbl_5d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 6: pnl_5.BackgroundImage = null;
                        lbl_5.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_5d.ForeColor = System.Drawing.Color.Orange;
                        pnl_6.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_6.ForeColor = System.Drawing.Color.White;
                        lbl_6d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 7: pnl_6.BackgroundImage = null;
                        lbl_6.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_6d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_7.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_7.ForeColor = System.Drawing.Color.White;
                        lbl_7d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 8: pnl_7.BackgroundImage = null;
                        lbl_7.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_7d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_8.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_8.ForeColor = System.Drawing.Color.White;
                        lbl_8d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 9: pnl_8.BackgroundImage = null;
                        lbl_8.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_8d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_9.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_9.ForeColor = System.Drawing.Color.White;
                        lbl_9d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 10: pnl_9.BackgroundImage = null;
                        lbl_9.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_9d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_10.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_10.ForeColor = System.Drawing.Color.White;
                        lbl_10d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 11: pnl_10.BackgroundImage = null;
                        lbl_10.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_10d.ForeColor = System.Drawing.Color.Orange;
                        pnl_11.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_11.ForeColor = System.Drawing.Color.White;
                        lbl_11d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 12: pnl_11.BackgroundImage = null;
                        lbl_11.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_11d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_12.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_12.ForeColor = System.Drawing.Color.White;
                        lbl_12d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 13: pnl_12.BackgroundImage = null;
                        lbl_12.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_12d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_13.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_13.ForeColor = System.Drawing.Color.White;
                        lbl_13d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 14: pnl_13.BackgroundImage = null;
                        lbl_13.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_13d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_14.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_14.ForeColor = System.Drawing.Color.White;
                        lbl_14d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;
                case 15: pnl_14.BackgroundImage = null;
                        lbl_14.ForeColor = System.Drawing.Color.DarkGray;
                        lbl_14d.ForeColor = System.Drawing.Color.DarkGray;
                        pnl_15.BackgroundImage = Properties.Resources.cauhoi_diemso;
                        lbl_15.ForeColor = System.Drawing.Color.White;
                        lbl_15d.ForeColor = System.Drawing.SystemColors.WindowFrame;
                        break;    
            }
        }
        private string diemso(int n)
        {
            switch (n)
            {
                case 1: return lbl_1d.Text;
                case 2: return lbl_2d.Text;
                case 3: return lbl_3d.Text;
                case 4: return lbl_4d.Text;
                case 5: return lbl_5d.Text;
                case 6: return lbl_6d.Text;
                case 7: return lbl_7d.Text;
                case 8: return lbl_8d.Text;
                case 9: return lbl_9d.Text;
                case 10: return lbl_10d.Text;
                case 11: return lbl_11d.Text;
                case 12: return lbl_12d.Text;
                case 13: return lbl_13d.Text;
                case 14: return lbl_14d.Text;
                case 15: return lbl_15d.Text;
            }
            return "";
        }
       
        private void btn_ready_Click(object sender, EventArgs e)
        {
            //2. Send n to server
            string n = "ready;" + account;
            Console.WriteLine(n);
            tcpForClient.SendData(n);
            btn_ready.Enabled = false;
        }
        private void pnl_khaosat_Click(object sender, EventArgs e)
        {

        }

        private void btn_aws_Click(object sender, EventArgs e)
        {
            //2. Send n to server
            string n = "aws;" + txt_1.Text + txt_2.Text + txt_3.Text + txt_4.Text + txt_5.Text + txt_6.Text;
            Console.WriteLine(n);
            tcpForClient.SendData(n);
            btn_aws.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label9.Text = count.ToString();
            count++;
            if (count == 15)
            {
                timer1.Stop();
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Hide();
            //2. Send n to server
            string n = "start;";
            Console.WriteLine(n);
            tcpForClient.SendData(n);
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            btn_play.Enabled = false;
            //2. Send n to server
            string str = "";
            if(rbn_A.Checked) str = "A";
            else if (rbn_B.Checked) str = "B";
            else if (rbn_C.Checked) str = "C";
            else if (rbn_D.Checked) str = "D";

            string n = "awsmp;" + str;
            Console.WriteLine(n);
            tcpForClient.SendData(n);
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
           
            btn_aws.Enabled = true;

            rbn_A.Enabled = true; lbl_A.Text = String.Empty;
            rbn_B.Enabled = true; lbl_B.Text = String.Empty; 
            rbn_C.Enabled = true; lbl_C.Text = String.Empty; 
            rbn_D.Enabled = true; lbl_D.Text = String.Empty; 
            //2. Send n to server
            string n = "next;";
            Console.WriteLine(n);
            tcpForClient.SendData(n);
            btn_next.Enabled = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Form frm = new GameOver(diemso(count));
            frm.ShowDialog();
            this.Close();
        }

        private void pnl_5050_Click(object sender, EventArgs e)
        {
            tcpForClient.SendData("c5050;");
            pnl_5050.Enabled = false;
        }

        private void btn_khangia_Click(object sender, EventArgs e)
        {
            tcpForClient.SendData("hoikhangia;");
            btn_khangia.Enabled = false;
        }

        private void pnl_nguoithan_Click(object sender, EventArgs e)
        {
            //pnl_nguoithan.Enabled = false;
            //Form frm = new Form6(lbl_ques.Text, lbl_A.Text, lbl_B.Text, lbl_C.Text, lbl_D.Text);
            //frm.ShowDialog();
        }

    }
}
