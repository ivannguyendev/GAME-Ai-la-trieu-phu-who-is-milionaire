using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ailatrieuphu
{
    class roominfo
    {
        string hovaten;
        string account;
        bool status;
        bool dung;
        int time;

        public roominfo()
        {
            hovaten = "";
            account = "";
            status = false;
            dung = false;
            time = 0;
        }
        public string Hovaten
        {
            get { return hovaten; }
            set { hovaten = value; }
        }
        public string Account
        {
            get { return account; }
            set { account = value; }
        }
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        public bool Dung
        {
            get { return dung; }
            set { dung = value; }
        }
        public int Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
