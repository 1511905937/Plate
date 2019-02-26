using MySql.Data.MySqlClient;
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
    public partial class User_D_deletedata : Form
    {
        public User_D_deletedata()
        {
            InitializeComponent();
        }

        private void User_D_deletedata_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.ShowUpDown = true;

            dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.ShowUpDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string time1 = dateTimePicker1.Value.ToString("yyyyMMddHHmmss");
                string time2 = dateTimePicker2.Value.ToString("yyyyMMddHHmmss");
                string sql = "delete from licence_plate where licence_plate_date between " + time1 + " and " + time2 + " ";
                delete(sql);
                sql = "selete picture_address from pictures where picture_date between " + time1 + " and " + time2 + "  ";
                deleteDir(sql);
                sql = "delete from pictures where picture_date between " + time1 + " and " + time2 + " ";
                delete(sql);
                sql = "selete video_address from videos where video_date between " + time1 + " and " + time2 + " ";
                deleteDir(sql);
                sql = "delete from videos where video_date between " + time1 + " and " + time2 + " ";
                delete(sql);
                MessageBox.Show("删除成功");
            }
            catch
            {

            }
        }

        private void delete(string sql)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteScalar();

                conn.Close();
               
            }
            catch
            {

            }
            
        }
        private void deleteDir(string sql)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Directory.Delete(reader[0].ToString(),true);
                }

                conn.Close();

            }
            catch
            {

            }
        }
    }
}
