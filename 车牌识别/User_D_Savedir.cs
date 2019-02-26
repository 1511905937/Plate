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
    public partial class User_D_Savedir : Form
    {
        public User_D_Savedir()
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
                mess[6] = textBox1.Text.ToString().Trim();
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
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择所有文件存放目录";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string sPath = folder.SelectedPath;
                textBox1.Text = sPath;
            }
        }

        private void User_D_Savedir_Load(object sender, EventArgs e)
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
            textBox1.Text = mess[6];
           
        }
    }
}
