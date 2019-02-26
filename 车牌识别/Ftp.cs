using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient;


namespace 车牌识别
{
    class Ftp
    {
        private long insert1(string sql)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conf.sqlconn);
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                //  MySqlDataReader reader = cmd.ExecuteReader();
                // long newid = Int64.Parse(reader[0].ToString());
                long newid = -1;
                newid=cmd.LastInsertedId;
                conn.Close();
                return newid;
            }
            catch
            {

            }
            return -1;
        }

        private void insert(string sql)
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

        //从ftp获得文件目录
        public void download_deal()
        {
            try
            {
                List<string> list = getList();
                if(list!=null)
                foreach (string s in list)
                {
                    System.Threading.Thread.Sleep(1000 * 20);//停留10秒
                    if (Directory.Exists(conf.pic_Path + "/" + s) == false)
                    {
                        Directory.CreateDirectory(conf.pic_Path + "/" + s);
                    }
                    getname_and_download(s);
                    Picture_deal P_d = new Picture_deal();
                    string path_c = conf.pic_Path + "\\" + s;
                    string path = path_c.Replace("\\", "/");
                    string num = P_d.deal("ECRECT.JPG", path,s);
                    if (num != "")
                    {
                        string a = num.Substring(0, 2);
                        string b= num.Substring(2, 5);
                        num = a + "·" + b;
                    }
                    try
                    {
                        string sql1 = "insert into licence_plate values(null,'" + num + "','" + s + "','','');";
                        long id = insert1(sql1);
                        string sql2 = "insert into pictures values(null,'" + id + "','" + path + "','" + s + "\')";
                        string sql3 = "insert into videos values(null,'" + id + "','" + path + "','" + s + "\')";
                        insert(sql2);
                        insert(sql3);
                      /*  if(Form1.pre!=null)
                         Form1.pre.showdatagridview(1);
                        else
                        {
                            Console.WriteLine("no");
                        }*/
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    //改记录时间
                    System.IO.File.WriteAllText(conf.time_Path, s, Encoding.UTF8);//conf.time.ToString("yyyy/MM/dd/HH:mm:ss"), Encoding.UTF8);
                }
            }
            catch
            {

            }
           
        }

        private List<string> getList()
        {  
            try
            {
                FtpWebRequest request;
                List<string> downloadFiles = new List<string>();
                //获取文件
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(conf.path));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(conf.username, conf.password);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.UseBinary = true;
                request.Timeout = 12000;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();

                while (line != null)
                {

                    string msg = line.Substring(56);
                    //  Console.WriteLine(msg);
                    string year = msg.Substring(0, 4);
                    string month = msg.Substring(4, 2);
                    string day = msg.Substring(6, 2);
                    string hour = msg.Substring(8, 2);
                    string minute = msg.Substring(10, 2);
                    string second = msg.Substring(12, 2);
                    string T = year + "/" + month + "/" + day + "/" + hour + "/" + minute + "/" + second;
                    conf.time = DateTime.ParseExact(T.ToString(), "yyyy/MM/dd/HH/mm/ss", System.Globalization.CultureInfo.CurrentCulture);
                    //onsole.WriteLine(T);
                    int ok = getTime();
                    Console.WriteLine(ok.ToString());
                    if (ok == 1)
                    {
                        Console.WriteLine(conf.time);
                        if (System.DateTime.Compare(conf.time, conf.Time) > 0)
                        {
                            downloadFiles.Add(msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("2");
                        downloadFiles.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return downloadFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件列表失败");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //从ftp获得文件目录
        private void getname_and_download(string dir)
        {
            FtpWebRequest request;
            try
            {
                //获取文件
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(conf.path + "/" + dir));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(conf.username, conf.password);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.UseBinary = true;
                request.Timeout = 12000;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();

                while (line != null)
                {
                    string filename = line.Substring(56).Trim();

                    DownloadFile(filename,dir);
                   

                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件列表失败");
           
            }


        }
        //删除图片
        public void DeleteFileName(string fileName)
        {
            try
            {
                FtpWebRequest request;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(conf.path+fileName));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(conf.username, conf.password);
                // 指定执行什么命令
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                
            }
        }

        //下载图片
        public void DownloadFile(string fileName,string dir)
        {
            FtpWebRequest reqFTP;
            try
            {
                string filePath = conf.pic_Path+"/"+dir;
                if (Directory.Exists(filePath) == false)//如果不存在就创建车牌图片文件夹
                {
                    Directory.CreateDirectory(filePath);
                }
                
                
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(conf.path +"/"+dir+"/"+ fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(conf.username, conf.password);
                reqFTP.Timeout = 30000;
                WebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("下载图片失败" + ex.ToString());
            }
        }
        //记录下载的最后一张图片的时间
      
        public int getTime()
        {
           
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(conf.time_Path, Encoding.Default);
                String l = sr.ReadLine();
                
                if (l == null)
                {
                    sr.Close();
                    return 0;
                }
                else
                {
                    conf.Time = DateTime.ParseExact(l.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    sr.Close();
                    Console.WriteLine(conf.Time);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
