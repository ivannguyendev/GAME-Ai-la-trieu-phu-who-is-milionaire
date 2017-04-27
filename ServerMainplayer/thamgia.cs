using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL
{
    class thamgia
    {
        string id, taikhoan, password, hoten, tuoi;
        nguoithan ngthan1 = new nguoithan();
        nguoithan ngthan2 = new nguoithan();
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Taikhoan
        {
            get { return taikhoan; }
            set { taikhoan = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Hoten
        {
            get { return hoten; }
            set { hoten = value; }
        }
        public string Tuoi
        {
            get { return tuoi; }
            set { tuoi = value; }
        }
        public nguoithan Ngthan1
        {
            get { return ngthan1; }
            set { 
                ngthan1.Id = value.Id;
                ngthan1.Id_tg = value.Id_tg;
                ngthan1.Hoten = value.Hoten;
                ngthan1.Quanhe = value.Quanhe;
            }
        }
        public nguoithan Ngthan2
        {
            get { return ngthan2; }
            set
            {
                ngthan2.Id = value.Id;
                ngthan2.Id_tg = value.Id_tg;
                ngthan2.Hoten = value.Hoten;
                ngthan2.Quanhe = value.Quanhe;
            }
        }
    }
    class nguoithan
    {
        string id, id_tg, hoten, quanhe;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Id_tg
        {
            get { return id_tg; }
            set { id_tg = value; }
        }
        public string Hoten
        {
            get { return hoten; }
            set { hoten = value; }
        }
        public string Quanhe
        {
            get { return quanhe; }
            set { quanhe = value; }
        }
    }
    
}
