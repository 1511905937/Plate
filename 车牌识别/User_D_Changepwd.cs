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
    public partial class User_D_Changepwd : Form
    {
        public User_D_Changepwd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Power p = new Power();
            int ok = p.changepwd(textBox3.Text.ToString().Trim(), textBox4.Text.ToString().Trim(),textBox1.Text.ToString().Trim(), textBox2.Text.ToString().Trim());
            if (ok == 1)
            {
                MessageBox.Show("修改成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }
    }
}
