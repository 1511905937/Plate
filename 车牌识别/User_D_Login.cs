using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 车牌识别
{
    public partial class User_D_Login : Form
    {
        public User_D_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Power p = new Power();
            int ok = p.login(textBox1.Text.ToString().Trim(),textBox2.Text.ToString().Trim());
            if(ok==1)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
