using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace warsha
{
    public partial class Add_Order : Form
    {
        public Add_Order()
        {
            InitializeComponent();
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            Home back_to_home  = new Home();
            back_to_home.Show();
            this.Hide();
        }
    }
}
