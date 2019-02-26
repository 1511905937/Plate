using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 车牌识别
{
    public partial class User_D_ftp : Form
    {
        public User_D_ftp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //读取信息
                FileStream f = new FileStream(conf.user_mess, FileMode.Open);
                long size = f.Length;
                byte[] array = new byte[size];
                f.Read(array, 0, array.Length);
                f.Close();
                //解码
                byte[] result = crypt.Decrypt(array, conf.key);
                //换存储格式
                string sresult = System.Text.Encoding.UTF8.GetString(result);
                string[] mess = sresult.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                mess[2] = textBox1.Text.ToString().Trim();
                mess[3] = textBox2.Text.ToString().Trim();
                mess[4] = textBox3.Text.ToString().Trim();
                mess[5] = textBox4.Text.ToString().Trim();
                //重新写入
                FileStream fs = new FileStream(conf.user_mess, FileMode.Create);
                string str = "";
                for (int i = 0; i < mess.Length; i++)
                {
                    str += mess[i];
                    str += "\r\n";
                }
                byte[] strarr = Encoding.UTF8.GetBytes(str);
                byte[] aresult = crypt.Encrypt(strarr, conf.key);
                fs.Write(aresult, 0, aresult.Length);
                fs.Close();

                conf.ftp_ip = textBox1.Text.ToString().Trim();
                conf.username = textBox2.Text.ToString().Trim();
                conf.password = textBox3.Text.ToString().Trim();
                conf.pic_dir = textBox4.Text.ToString().Trim();
                conf.path = @"ftp://" + conf.ftp_ip + "/" + conf.pic_dir;
                this.Close();
            }
            catch
            {

            }
        }

        private void User_D_ftp_Load(object sender, EventArgs e)
        {
            textBox1.Text = conf.ftp_ip;
            textBox2.Text = conf.username;
            textBox3.Text = conf.password;
            textBox4.Text = conf.pic_dir;
        }
    }
}
