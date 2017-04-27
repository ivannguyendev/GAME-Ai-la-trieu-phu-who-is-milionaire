using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace SQL
{
    class ctrl_thamgia
    {
        thamgia thamgia = new thamgia();
        sqlserver sqldata = new sqlserver();
        DataSet ds = new DataSet();//Đối tượng chứa dữ liệu tại local
       // public DataTable ShowLop(); //Định nghĩa hàm ShowLop() kiểu DataTable


       // public void UpdateLop(string ml1, string ml, string tl) ;//Thực thi câu lệnh cập nhật

      //  public void DeleteLop(string ml) ;//Thực thi xóa lớp

      //  public DataTable lookLop(string dk); // Thực thi tìm kiếm

        public bool insert(thamgia tg)
        {
            //INSERT INTO THAMGIA VALUES( 1 ,'ADMIN', 'ADMIN' ,'Admin' , '18' )
            
            string sql1 = "INSERT INTO THAMGIA (ID_TG,TAIKHOAN,PASS,HOTEN,TUOI) VALUES( N'" + tg.Id + 
                        "', N'" + tg.Taikhoan + 
                        "', N'" + tg.Password + 
                        "', N'" + tg.Hoten + 
                        "', N'" + tg.Tuoi + "')" 
                        ;
            string sql2 = "INSERT INTO NGTHAN (ID_NGT,ID_TG,HOTEN,QUANHE) VALUES( N'" + tg.Ngthan1.Id +
                         "', N'" + tg.Ngthan1.Id_tg +
                         "', N'" + tg.Ngthan1.Hoten +
                         "', N'" + tg.Ngthan1.Quanhe + "')"                    
                        ;
            string sql3 = "INSERT INTO NGTHAN (ID_NGT,ID_TG,HOTEN,QUANHE) VALUES( N'" + tg.Ngthan2.Id +
                        "', N'" + tg.Ngthan2.Id_tg +
                        "', N'" + tg.Ngthan2.Hoten +
                        "', N'" + tg.Ngthan2.Quanhe + "')"
                        ;
            try
            {
                sqldata.ExecuteNonQuery(sql1);
                sqldata.ExecuteNonQuery(sql2);
                sqldata.ExecuteNonQuery(sql3);
                Console.WriteLine("thêm dữ liệu thành công\n");
            }
            catch
            {
                Console.WriteLine("lỗi thêm dữ liệu \n");
                return false;
            }
            return true;
        } // Chèn dữ liệu từ form đăng kí
        public bool login (string account = "", string pass = "")
        {
            //string sql = "SELECT ID_TG, HOTEN, TUOI FROM THAMGIA WHERE TAIKHOAN='" +
            //              account +
            //              "' and PASS= '" + pass + "'"
            //              ;
                                // 1.  create a command object identifying
                                //     the stored procedure

            SqlCommand cmd = new SqlCommand("CHECKLOGIN", sqldata.getconnect());
                                // 2. set the command object so it knows
                                //    to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;
                                // 3. add parameter to command, which
                                //    will be passed to the stored procedure
            cmd.Parameters.Add(new SqlParameter("@TAIKHOAN", account));
            cmd.Parameters.Add(new SqlParameter("@pass", pass));
            SqlConnection sql = sqldata.getconnect();
            cmd.Connection = sql;
            using (var reader = sqldata.ExecuteReader(cmd))
            {
                // iterate through results, printing each to console
                while (reader.Read())
                {
                    Console.WriteLine("{0}\n",reader.GetValue(0));
                    int t = int.Parse(reader.GetValue(0).ToString());
                    if ( t != 1)  return false;
                }
            }
            thamgia.Taikhoan = account;
            sql.Close();
            //getinfor();
            return true;
        } // hàm xác nhận tài khoản

        private void getinfor()
        {
            string sql = "SELECT ID_TG, TAIKHOAN, HOTEN, TUOI  FROM THAMGIA WHERE TAIKHOAN = '" + thamgia.Taikhoan + "'";
            //sqldata.getconnect().Open();
            using (SqlDataAdapter adap = sqldata.getadapter(sql))
            {
                adap.Fill(ds, "THAMGIA");
                Console.WriteLine("ds-------{0}\n", ds.Tables["THAMGIA"].Rows[3].ToString());
            }
            //sqldata.getconnect().Close();         
        }

    }
}
