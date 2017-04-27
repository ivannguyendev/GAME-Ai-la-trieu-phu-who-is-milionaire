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
    public partial class GameOver : Form
    {
        public GameOver()
        {
            InitializeComponent();
        }
        public GameOver(string n)
        {
            InitializeComponent();
            label3.Text = n;
            if (n == "150000") label1.Text = "Bạn là người chiến thắng!\n Chúc bạn luôn thành công trong cuộc sống!\n Xin chào và hẹn gặp lại!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
