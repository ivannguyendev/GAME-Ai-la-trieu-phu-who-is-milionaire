using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQL
{
    class sqlserver
    {
        string datasource = @"Data Source=IVAN-UIT\IVANSERVER;";
        string catalog = "Initial Catalog=ailatrieuphu;";
        string intergrated = "Integrated Security=True;";

        public SqlConnection getconnect()
        {
            try
            {
                return new SqlConnection(
                                @"Data Source=IVAN-UIT\IVANSERVER;" +
                    //"AttachDbFilename=@DATABASE\ailatrieuphu_data_Data.mdf" +
                                "Initial Catalog=ailatrieuphu;" +
                                "Integrated Security=True;"
                    // "Min Pool Size = 5;" +
                    // "Max Pool Size = 15;" +
                    //  "Connect Reset = True;" +
                    // "Connection Lifetime = 600;"
                );
                
            }
            catch
            {
                Console.WriteLine("Kết nối ServerDatabase thất bại");
                return null;
            }
            return null;
        }
        public SqlConnection getconnect(
            string datasource ,
            string catalog ,
            string intergrated
            //string pool = "false"
            )
        {

            try
            {
                return new SqlConnection(datasource + catalog + intergrated //+ "pool = " + pool
                    //"Initial Catalog=ailatrieuphu;" +
                    //"Integrated Security=True"
                    //"Min Pool Size = 5;" +
                    //"Max Pool Size = 15" +
                    //"Connect Reset = True" +
                    //"Connection Lifetime = 600";
                );
                
            }
            catch
            {
                Console.WriteLine("Kết nối ServerDatabase thất bại");
                return null;
            }
            return null;
        }
        public SqlDataAdapter getadapter(string sql)
        {

            SqlDataAdapter adap = new SqlDataAdapter();//Khai báo đối tượng gắn kết DataSource với DataSet
            //Khởi tạo đối tượng DataAdapter và cung cấp vào câu lệnh SQL và đối tượng Connection
            adap.SelectCommand = new SqlCommand(sql, getconnect());
            //Data Adapter sẽ tự động sinh ra khóa chính từ khóa chính ở Database
            adap.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            return adap;
        }// Lấy dữ liệu về local
        public SqlDataReader ExecuteReader(SqlCommand cmd)//Thực thi các câu lệnh Select trực tiếp từ database online mode
        {
                SqlDataReader reader;
                cmd.Connection = getconnect();
                cmd.Connection.Open();
                reader = cmd.ExecuteReader();
                    //Console.WriteLine("Đã insert {0} \n", i);
                return reader;
        }
        public void ExecuteNonQuery(string sql)//Thực thi các câu lệnh Insert, update, Delete
        {
                
            //try
            //{
                using (SqlCommand cmd = new SqlCommand(sql,getconnect()))
                {
                    cmd.Connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    Console.WriteLine("Đã insert {0} \n", i);
                    cmd.Dispose();
                }
                
            //}
            //catch
            //{
            //    Console.WriteLine("Lỗi thêm dữ liệu \n");
            //}
            
        }

    } 
}
