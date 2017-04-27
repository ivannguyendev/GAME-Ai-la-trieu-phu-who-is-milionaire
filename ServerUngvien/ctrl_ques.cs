using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace SQL
{
    class ctrl_ques
    {
        question_uv posques_ungvien  = new question_uv();
        question[] posques1_5 = new question[5];
        question[] posques6_10  = new question[5];
        question[] posques11_15 = new question[5];
        sqlserver sqldata;
        DataSet ds;//Đối tượng chứa dữ liệu tại local

        public ctrl_ques()
        {
            //Connect database
            sqldata = new sqlserver();
           
        }
        
        private void loadques_trungbinh()
        {
            // Truy vấn dữ liệu SELECT từ bảng câu hỏi từ 6-10
            string sql = "SELECT TOP 5 ID_MP, NOIDUNG, DUNG, DAA, DAB, DAC, DAD  FROM QUESTION_TRUNGBINH ORDER BY NEWID()";
            sqldata.getconnect().Open();
            using (SqlDataAdapter adapter = sqldata.getadapter(sql))
            {
                adapter.Fill(ds, "dsQUESTION_TB");
                //Load dữ liệu từ dataset dữ liệu local
                for (int row = 0; row < 5; row++)
                {
                    posques6_10[row] = new question();
                    posques6_10[row].Id = ds.Tables["dsQUESTION_TB"].Rows[row]["ID_MP"].ToString();
                    posques6_10[row].Noidung = ds.Tables["dsQUESTION_TB"].Rows[row]["NOIDUNG"].ToString();
                    posques6_10[row].Dapandung = ds.Tables["dsQUESTION_TB"].Rows[row]["DUNG"].ToString();
                    posques6_10[row].Dapan1 = ds.Tables["dsQUESTION_TB"].Rows[row]["DAA"].ToString();
                    posques6_10[row].Dapan2 = ds.Tables["dsQUESTION_TB"].Rows[row]["DAB"].ToString();
                    posques6_10[row].Dapan3 = ds.Tables["dsQUESTION_TB"].Rows[row]["DAC"].ToString();
                    posques6_10[row].Dapan4 = ds.Tables["dsQUESTION_TB"].Rows[row]["DAD"].ToString();
                    Console.WriteLine("{0} {1}", posques6_10[row].Noidung, posques6_10[row].Dapandung);
                }
            }
            sqldata.getconnect().Close();
        }
        private void loadques_de()
        {
            string sql = "SELECT TOP 5 ID_MP, NOIDUNG, DUNG, DAA, DAB, DAC, DAD  FROM QUESTION_DE ORDER BY NEWID()";
            sqldata.getconnect().Open();
            using (SqlDataAdapter adapter = sqldata.getadapter(sql))
            {
                adapter.Fill(ds, "dsQUESTION_DE"); 
                for (int row = 0; row < ds.Tables["dsQUESTION_DE"].Rows.Count; row++)
                {
                    //t.Id= ds.Tables["QUESTION_DE"].Rows[row]["ID_MP"].ToString();
                    //Console.WriteLine("{0}", t.id);
                    posques1_5[row] = new question();
                    posques1_5[row].Id = ds.Tables["dsQUESTION_DE"].Rows[row]["ID_MP"].ToString();
                    posques1_5[row].Noidung = ds.Tables["dsQUESTION_DE"].Rows[row]["NOIDUNG"].ToString();
                    posques1_5[row].Dapandung = ds.Tables["dsQUESTION_DE"].Rows[row]["DUNG"].ToString();
                    posques1_5[row].Dapan1 = ds.Tables["dsQUESTION_DE"].Rows[row]["DAA"].ToString();
                    posques1_5[row].Dapan2 = ds.Tables["dsQUESTION_DE"].Rows[row]["DAB"].ToString();
                    posques1_5[row].Dapan3 = ds.Tables["dsQUESTION_DE"].Rows[row]["DAC"].ToString();
                    posques1_5[row].Dapan4 = ds.Tables["dsQUESTION_DE"].Rows[row]["DAD"].ToString();
                    Console.WriteLine("{0} {1}", posques1_5[row].Noidung, posques1_5[row].Dapandung);
                }
            }
            sqldata.getconnect().Close();
         }
        private void loadques_kho()
        {
            string sql = "SELECT TOP 5 ID_MP, NOIDUNG, DUNG, DAA, DAB, DAC, DAD  FROM QUESTION_KHO ORDER BY NEWID()";
            sqldata.getconnect().Open();
            using (SqlDataAdapter adapter = sqldata.getadapter(sql))
            {
                adapter.Fill(ds, "dsQUESTION_KHO");
                for (int row = 0; row < 5; row++)
                {
                    posques11_15[row] = new question();
                    posques11_15[row].Id = ds.Tables["dsQUESTION_KHO"].Rows[row]["ID_MP"].ToString();
                    posques11_15[row].Noidung = ds.Tables["dsQUESTION_KHO"].Rows[row]["NOIDUNG"].ToString();
                    posques11_15[row].Dapandung = ds.Tables["dsQUESTION_KHO"].Rows[row]["DUNG"].ToString();
                    posques11_15[row].Dapan1 = ds.Tables["dsQUESTION_KHO"].Rows[row]["DAA"].ToString();
                    posques11_15[row].Dapan2 = ds.Tables["dsQUESTION_KHO"].Rows[row]["DAB"].ToString();
                    posques11_15[row].Dapan3 = ds.Tables["dsQUESTION_KHO"].Rows[row]["DAC"].ToString();
                    posques11_15[row].Dapan4 = ds.Tables["dsQUESTION_KHO"].Rows[row]["DAD"].ToString();
                    Console.WriteLine("{0} {1}", posques11_15[row].Noidung, posques11_15[row].Dapandung);
                }
            }
            sqldata.getconnect().Close();
        }

        public void loadques_mainplayer()
        {
            ds = new DataSet();
            loadques_de();
            loadques_kho();
            loadques_trungbinh();
        } //Hàm load câu hỏi cho người chơi chính
        public question_uv getques_player()
        {
            ds = new DataSet();
            string sql = "SELECT ID_UV, NOIDUNG, DUNG, DA1, DA2, DA3, DA4, DA5, DA6 FROM QUESTION_UNGVIEN ORDER BY NEWID()";
            sqldata.getconnect().Open();
            using (SqlDataAdapter adapter = sqldata.getadapter(sql))
            {
                adapter.Fill(ds, "dsQUESTION_UV");
                posques_ungvien.Id = ds.Tables["dsQUESTION_UV"].Rows[0]["ID_UV"].ToString();
                posques_ungvien.Noidung = ds.Tables["dsQUESTION_UV"].Rows[0]["NOIDUNG"].ToString();
                posques_ungvien.Dapandung = ds.Tables["dsQUESTION_UV"].Rows[0]["DUNG"].ToString();
                posques_ungvien.Dapan1 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA1"].ToString();
                posques_ungvien.Dapan2 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA2"].ToString();
                posques_ungvien.Dapan3 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA3"].ToString();
                posques_ungvien.Dapan4 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA4"].ToString();
                posques_ungvien.Dapan5 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA5"].ToString();
                posques_ungvien.Dapan6 = ds.Tables["dsQUESTION_UV"].Rows[0]["DA6"].ToString();
            }
            sqldata.getconnect().Close() ;
            return posques_ungvien;
        } //hàm lấy dữ liệu câu hỏi cho ứng viên
        public question getques_mainplayer(int pos)
        {
            pos--;
            if (pos < 5 && pos > -1) return posques1_5[pos];
            else if (pos >= 5 && pos < 11) return posques6_10[pos - 5];
            return posques11_15[pos - 11];
        } //Hàm lấy câu hỏi cho người chơi chính
        
    }
}
