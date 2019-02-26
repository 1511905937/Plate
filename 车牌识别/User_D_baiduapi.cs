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
    public partial class User_D_baiduapi : Form
    {
        public User_D_baiduapi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
            mess[7] = textBox1.Text.ToString().Trim();
            mess[8] = textBox2.Text.ToString().Trim();
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
            this.Close();
        }

        private void User_D_baiduapi_Load(object sender, EventArgs e)
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
            textBox1.Text = mess[7];
            textBox2.Text = mess[8];
        }
    }
}
