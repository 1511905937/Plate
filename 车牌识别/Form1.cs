using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Net;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace 车牌识别
{
    public partial class Form1 : Form
    {

        Ftp ftp = new Ftp();
        Picture_deal P_d = new Picture_deal();

        public Form1()
        {
            this.MaximizeBox = false;  //窗口禁止最大化

            initial();

            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void initial()
        {
            try
            {
                //创建文件
                if (File.Exists(conf.user_mess)==false)
                {
                    FileStream f = new FileStream(conf.user_mess, FileMode.Create);
                    f.Close();
                }
                //判断文件是否空文件
                StreamReader sr = null;
                sr = new StreamReader(conf.user_mess, Encoding.UTF8);
                string l = sr.ReadLine();
                sr.Close();
                //如果为空，初始化
                if (l == null)
                {
                    FileStream fs = new FileStream(conf.user_mess, FileMode.Append);
                    string str = "juteng\r\njuteng\r\n*\r\n*\r\n*\r\n*\r\n*\r\n*\r\n*\r\n";
                    byte[] strarr = Encoding.UTF8.GetBytes(str);
                    byte[] result = crypt.Encrypt(strarr,conf.key);
                    fs.Write(result, 0, result.Length);
                    fs.Close();
                }
                else  //读取信息
                {
                    //读取信息
                    FileStream fs = new FileStream(conf.user_mess, FileMode.Open);
                    long size = fs.Length;
                    byte[] array = new byte[size];
                    fs.Read(array, 0, array.Length);
                    fs.Close();
                    //解码
                    byte[] result = crypt.Decrypt(array, conf.key);
                    //换存储格式
                    string sresult = System.Text.Encoding.UTF8.GetString(result);
                    string[] mess = sresult.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    //提取信息
                    conf.ftp_ip = mess[2];
                    conf.username = mess[3];
                    conf.password = mess[4];
                    conf.pic_dir = mess[5];
                    conf.main_Path = mess[6];
                    conf.path = @"ftp://" + conf.ftp_ip + "/" + conf.pic_dir + "/AAA000/";
                    conf.pic_Path = conf.main_Path + "\\车牌图片";
                    conf.pic_d_Path = conf.main_Path + "\\已识别的车牌图片";   
                    conf.doc_Path = conf.main_Path + "\\车牌识别.txt";

                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            try
            {
                object obj = "";  //提示信息
                for (int i = 0; ; i++)
                {
                    //获取ftp文件列表
                    obj = "正在下载";
                    backgroundWorker1.ReportProgress(1, obj);
                    ftp.download_deal();
                    obj = "识别完成";
                    backgroundWorker1.ReportProgress(1, obj);

                    //停留十秒
                    System.Threading.Thread.Sleep(1000 * 10); //停留10秒
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                label2.Text = e.UserState.ToString();
                label2.Update();
            }
            catch
            {

            }
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
             
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        void login_after(object sender, EventArgs e)
        {
            try
            {
                Power p = new Power();

                if (Power.power == 1)
                {
                    button3.Text = "已登录";
                    button3.Update();
                }
            }
            catch
            {

            }
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                User_D_Login login = new User_D_Login();
                login.Show();
                login.FormClosed += new FormClosedEventHandler(login_after);
            }
            catch
            {

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_Changepwd changepwd = new User_D_Changepwd();
                    changepwd.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_ftp ftp = new User_D_ftp();
                    ftp.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    initial();
                    if (conf.main_Path != "" && conf.username != "" && conf.password != "" && conf.path != "")
                    {
                        backgroundWorker1.WorkerReportsProgress = true;
                        backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                        backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
                        backgroundWorker1.RunWorkerAsync();

                        if (Directory.Exists(conf.main_Path) == false)//如果不存在就创建文件夹
                        {
                            Directory.CreateDirectory(conf.main_Path);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    Power.power = 0;
                    button3.Text = "登录";
                    button3.Update();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_Savedir Savedir = new User_D_Savedir();
                    Savedir.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_Setaddress setaddress = new User_D_Setaddress();
                    setaddress.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_baiduapi api = new User_D_baiduapi();
                    api.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }
      //  public static preview pre = null;
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                /*   if (Power.power == 1)
                   {
                       if(pre==null)
                       {
                           pre = new preview();
                           pre.Show();

                       }
                      else
                       {
                           pre.Activate();
                           pre.Show();
                       }
                
                   
                }
                else
                {
                    MessageBox.Show("请先登录");
                }*/
                preview pre = new preview();
                pre.Show();
            }
            catch
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //改记录时间
           // System.IO.File.WriteAllText(conf.time_Path, conf.time.ToString("yyyy/MM/dd/HH:mm:ss"), Encoding.UTF8);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (Power.power == 1)
                {
                    User_D_deletedata del = new User_D_deletedata();
                   
                    del.Show();
                }
                else
                {
                    MessageBox.Show("请先登录");
                }
            }
            catch
            {

            }
            
        }
    }
}
