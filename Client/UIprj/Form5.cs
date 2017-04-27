using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        public Form5(string str)
        {
            InitializeComponent();
            string[] str_split = str.Split(';');
            label5.Text = str_split[1];
            label6.Text = str_split[2];
            label7.Text = str_split[3];
            label8.Text = str_split[4];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
