using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace 车牌识别
{
    public partial class preview : Form
    {
        public preview()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        public string nowtime = "0";
      
        private void preview_Load(object sender, EventArgs e)
        {
            try
            {

                StreamReader sr = new StreamReader(conf.time_Path, Encoding.Default);
                String l = sr.ReadLine();
                if (l != null)
                {
                    nowtime = l.ToString();
                }
                sr.Close();

                dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.ShowUpDown = true;

                dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.ShowUpDown = true;


                showdatagridview(1);

                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork_1);
                backgroundWorker1.RunWorkerAsync();

             
            }
            catch
            {

            }
            

        }
      
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string chepai_id = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string sql1 = "select picture_address,picture_date from pictures where licence_plate_name=" + chepai_id + "";
                ShowPicture(sql1);
                string sql2 = "select video_address,video_date from videos where licence_plate_name=" + chepai_id + "";
                ShowVideo(sql2);

            }
            catch
            {

            }
           
        }

        private string showtime(string msg)
        {
            try
            {
                string year = msg.Substring(0, 4);
                string month = msg.Substring(4, 2);
                string day = msg.Substring(6, 2);
                string hour = msg.Substring(8, 2);
                string minute = msg.Substring(10, 2);
                string second = msg.Substring(12, 2);
                string T = year + "/" + month + "/" + day + " " + hour + ":" + minute + ":" + second;
                return T;
            }
            catch
            {

            }
            return "";
        }
        private void ShowVideo(string sql)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string msg = reader[1].ToString();
                    string T = showtime(msg);

                    axWindowsMediaPlayer1.URL = reader[0].ToString() + "/AAA000" + reader[1].ToString() + "TTAC.avi";

                }
                conn.Close();
            }
            catch
            {

            }
           
        }
        private void ShowPicture(string sql)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string msg = reader[1].ToString();
                    string T = showtime(msg);


                    this.pictureBox1.Load(reader[0].ToString() + "/ECRECT.jpg");
                  //  this.pictureBox2.Image = Image.FromFile(reader[0].ToString() + "/AAA000" + reader[1].ToString() + "DH.png");
                    this.pictureBox3.Image = Image.FromFile(reader[0].ToString() + "/AAA000" + reader[1].ToString() + "AC.jpg");
                  //  this.pictureBox4.Image = Image.FromFile(reader[0].ToString() + "/AAA000" + reader[1].ToString() + "CH.png");
                    this.pictureBox5.Image = Image.FromFile(reader[0].ToString() + "/AAA000" + reader[1].ToString() + "BC.jpg");
                    //this.pictureBox6.Image = Image.FromFile(reader[0].ToString() + "/AAA000" + reader[1].ToString() + "EH.png");

                }
                conn.Close();
            }
            catch
            {

            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                string s = pictureBox1.ImageLocation;
                // MessageBox.Show(s);
                if (s != null)
                {
                    picture pic = new picture(s);
                    pic.StartPosition = FormStartPosition.CenterScreen;
                    pic.Show();
                }
            }
            catch
            {

            }
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private long mark = 0;
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            /*if (e.newState == 1)
            {
                updatevideo();
            }*/
        }

        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMddHHmmss");
                string time2 = dateTimePicker2.Value.ToString("yyyyMMddHHmmss");
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                conn.Open();
                MySqlDataAdapter sda = new MySqlDataAdapter("select licence_plate_id,licence_plate_address,licence_plate_name,licence_plate_date from licence_plate where licence_plate_date between " + time1 + " and " + time2 + " order by licence_plate_date asc limit 200 ", conn);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "licence_plate_name");

                //使用DataSet绑定时，必须同时指明DateMember 
                this.dataGridView1.DataSource = Ds;

                this.dataGridView1.DataMember = "licence_plate_name";
                this.dataGridView1.Columns[1].HeaderText = "序号";
                this.dataGridView1.Columns[1].Width = 53;
                this.dataGridView1.Columns[2].HeaderText = "车牌";
                this.dataGridView1.Columns[2].Width = 110;
                this.dataGridView1.Columns[2].DefaultCellStyle.Font = new Font("宋体", 15);
                this.dataGridView1.Columns[3].HeaderText = "日期";
                this.dataGridView1.Columns[3].Width = 127;
                this.dataGridView1.RowHeadersVisible = false;
                this.dataGridView1.Columns[0].Visible = false;
     


                //自动整理序列号
                int coun = dataGridView1.RowCount;
                for (int i = 0; i < coun; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = i + 1;
                }


                for (int i = 0; i < coun; i++)
                {
                    string T = showtime(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    dataGridView1.Rows[i].Cells[3].Value = T;
                }

                label2.Text = "共" + coun.ToString() + "个";
                label2.Update();
                conn.Close();
            }
            catch
            {

            }
            
        }

        private void updatevideo()
        {
            try
            {
                DirectoryInfo TheFolder = new DirectoryInfo(conf.pic_Path);
                //遍历文件夹
                long max = 0,max_=0;
                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                {
                    string date = NextFolder.Name;
                    long date_c = Int64.Parse(date);
                    if (date_c > max)
                    {
                        max_ = max;
                        max = date_c;
                    }
                }
                int state = (int)axWindowsMediaPlayer1.playState;
               // MessageBox.Show("++"+mark.ToString());
                if (state!=3&&state!=6&&state!=9)
                {
                    if (max != mark)
                    {
                      
                        for(int i=0;i<3 ;i++)
                        {

                            int ok=played(max);
                           // MessageBox.Show(ok.ToString());
                            if (ok == 1)
                            {
                               
                                mark = max;
                             //   MessageBox.Show(mark.ToString());
                                break;
                            }
                                
                            System.Threading.Thread.Sleep(1000 * 30); //停留10秒
                        }
                        
                    }
                }
               
            }
            catch
            {

            }
        }
        private int played(long max)
        {
            int ok = 0;
            try
            {
               
                axWindowsMediaPlayer1.URL = conf.pic_Path + "\\" + max + "\\AAA000" + max + "TTAC.avi";
                axWindowsMediaPlayer1.Ctlcontrols.play();
                int state = (int)axWindowsMediaPlayer1.playState;
                if (state == 3 || state == 6 || state == 9||state==2)
                    ok = 1;
                this.pictureBox1.Load(conf.pic_Path + "\\" + max + "/ECRECT.jpg");
             //   this.pictureBox2.Image = Image.FromFile(conf.pic_Path + "\\" + max + "/AAA000" + max + "BH.png");
                this.pictureBox3.Image = Image.FromFile(conf.pic_Path + "\\" + max + "/AAA000" + max + "AC.jpg");
             //   this.pictureBox4.Image = Image.FromFile(conf.pic_Path + "\\" + max + "/AAA000" + max + "CH.png");
                this.pictureBox5.Image = Image.FromFile(conf.pic_Path + "\\" + max + "/AAA000" + max + "BC.jpg");
               // this.pictureBox6.Image = Image.FromFile(conf.pic_Path + "\\" + max + "/AAA000" + max + "EH.png");
               
                return ok;
            }
            catch
            {

                return 0;
            }
            
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            try
            {

                for (int i = 0; ; i++)
                {
                    updatevideo();
                    System.Threading.Thread.Sleep(1000 * 20); //停留20秒   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       
        public void showdatagridview(int ok)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                BindingSource bs = new BindingSource();
                MySqlDataAdapter sda = new MySqlDataAdapter("select licence_plate_id,licence_plate_address,licence_plate_name,licence_plate_date from licence_plate where licence_plate_date>"+nowtime+" order by licence_plate_date desc limit 500 ", conn);
                DataSet Ds = new DataSet();
                if (ok == 1)
                {
                    conn.Open();
                    sda.Fill(Ds, "licence_plate_name");

                   
                    bs.DataSource = Ds;
                    bs.ResetBindings(false);
                    
                    this.dataGridView1.DataSource = bs;
                   // this.dataGridView1.DataSource = Ds;
                    this.dataGridView1.DataMember = "licence_plate_name";
                    this.dataGridView1.Columns[1].HeaderText = "序号";
                    this.dataGridView1.Columns[1].Width = 53;
                    this.dataGridView1.Columns[2].HeaderText = "车牌";
                    this.dataGridView1.Columns[2].Width = 110;
                    this.dataGridView1.Columns[2].DefaultCellStyle.Font = new Font("宋体", 15);
                    this.dataGridView1.Columns[3].HeaderText = "日期";
                    this.dataGridView1.Columns[3].Width = 127;
                    this.dataGridView1.RowHeadersVisible = false;
                    this.dataGridView1.Columns[0].Visible = false;



                    //自动整理序列号
                    int coun = dataGridView1.RowCount;
                    for (int i = 0; i < coun; i++)
                    {
                        dataGridView1.Rows[i].Cells[1].Value = i + 1;
                    }


                    for (int i = 0; i < coun; i++)
                    {
                        string T = showtime(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        dataGridView1.Rows[i].Cells[3].Value = T;
                    }

                    label2.Text = "共" + coun.ToString() + "个";
                    label2.Update();

                    conn.Close();
                }
                else
                {
                   
                }


            }
            catch
            {


            }
        }





        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
         
        }

        private void axWindowsMediaPlayer1_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_DoubleClickEvent e)
        {
         
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                showdatagridview(1);
            }
            catch
            {

            }
            
        }

      
        private void OrderTimer_Tick(object sender, EventArgs e)
        {
          
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
