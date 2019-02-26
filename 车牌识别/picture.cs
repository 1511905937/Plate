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
    public partial class picture : Form
    {
        private string path = "";
        public picture(string s)
        {
            InitializeComponent();
            path = s;
        }

        private void picture_Load(object sender, EventArgs e)
        {
            try
            {
                if (path != "")
                {
                    pictureBox1.Image = Image.FromFile(path);
                    System.Drawing.Image localimage = System.Drawing.Image.FromFile(path);
                    //localimage.Width, localimage.Height
                    pictureBox1.Width = localimage.Width;
                    pictureBox1.Height = localimage.Height;
                    this.Width = localimage.Width;
                    this.Height = localimage.Height;
                }
            }
            catch
            {

            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
