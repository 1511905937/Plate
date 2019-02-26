
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime;


namespace 车牌识别
{
    class Picture_deal
    {
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

        //输出数据
        private string print_out(String json,string s)
        {
            try
            {
                System.DateTime currentTime = System.DateTime.Now;
                string T = showtime(s);
                JObject jo = (JObject)JsonConvert.DeserializeObject(json);
                if (jo.Property("words_result") != null)
                {
                    string num = jo["words_result"]["number"].ToString();
                    string title = "";
                    if (!System.IO.File.Exists(conf.doc_Path))
                    {
                        title = "时间 地点 车牌信息";
                    }
                    FileStream fs = new FileStream(conf.doc_Path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    if (title != "")
                        sw.WriteLine(title);
                    sw.WriteLine(T + " " + "郑州市郑新快速路与祥云路交叉口西100米" + " " + num);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    return num;
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine("输出识别结果失败");

            }
            return "";
        }

        public string deal(string filename, string dir, string s)
        {
            Bitmap pic = null;         //图片
            string results = "";    //识别结果
            string deal_path = "";  //文件路径

            //设置文件路径
            try
            {
                deal_path = dir + "\\" + filename;
                Console.WriteLine(deal_path);
                //识别图片
                pic = new Bitmap(deal_path);
                byte[] image = BitmapToByte(pic);
                var client = new Baidu.Aip.Ocr.Ocr("mw9MavWMGqrvzIGlU0g3BiV1", "jFYqSHf3pIUiFwKo3hFf0WRwGcw4LLVP");
                var result = client.PlateLicense(image);
                results = result.ToString();
                //输出数据
                string num = print_out(results, s);
                // Add_Word_And_Save(pic, filename, results);
                return num;
            }
            catch
            {
                return "";
            }
        }

        public void Add_Word_And_Save(Bitmap pic,String filename,String results)
        {
            string num = "";//车牌号
            string dir = "";//车牌文件夹
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(results);
                if (jo.Property("words_result") != null)
                {
                    num = jo["words_result"]["number"].ToString();
                    dir = conf.pic_d_Path + "\\" + num;
                    //创建文件夹
                    if (Directory.Exists(conf.pic_d_Path) == false)
                    {
                        Directory.CreateDirectory(conf.pic_d_Path);
                    }
                    //创建文件夹
                    if (Directory.Exists(dir) == false)
                    {
                        Directory.CreateDirectory(dir);
                    }
                    //生成随机数防伪码
                    String fangweima = "";
                    Random rd = new Random();
                    char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
                    System.Text.StringBuilder newRandom = new System.Text.StringBuilder(36);
                    for (int i = 0; i < 32; i++)
                    {
                        newRandom.Append(constant[rd.Next(36)]);
                    }
                    fangweima = newRandom.ToString();
                    //获取当前时间
                    System.DateTime currentTime = System.DateTime.Now;
                    currentTime = currentTime.AddSeconds(-30);
                    //图片上添加字体
                    int width = pic.Width;
                    int height = pic.Height;
                    Graphics g = Graphics.FromImage(pic);
                    String str = "设备编号：410201805060001 \r\n抓拍时间：" + currentTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n地点：郑州市郑新快速路与祥云路交叉口西200米 西向东第二车道\r\n防伪码：" + fangweima + "\r\n违法行为：违规使用远光灯\r\n";
                    Font font = new Font("宋体", 25);
                    SolidBrush sbrush = new SolidBrush(Color.White);
                    g.DrawString(str, font, sbrush, new PointF(60, 60));
                    pic.Save(dir+ "\\" + filename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("输出识别结果失败");

            }
        }

        //bitmap转为二进制
        public byte[] BitmapToByte(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, ImageFormat.Jpeg);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                ms.Close();
                return byteImage;
            }
            catch
            {
                ms.Close();
                return null;
            }
        }
    }
}
