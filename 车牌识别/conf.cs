using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 车牌识别
{
    class conf
    {
        //ftp信息
        public static string ftp_ip = "";  //ftp地址
        public static string username = "";  //ftp用户名
        public static string password = ""; //ftp密码
        public static string pic_dir = "";  //ftp目录
        public static string path = "";   //目标路径

        public static string main_Path = "";          //信息所存储的位置
        public static string pic_Path = main_Path+"\\车牌图片";   //下载的图片位置
        public static string pic_d_Path = main_Path + "\\已识别的车牌图片";   //添加时间后的车牌图片
        public static string doc_Path = main_Path + "\\车牌识别.txt";   //车牌号文本位置
        public static string time_Path = "time.txt";       //记录下载的最后一张图片的时间
        public static System.DateTime time = new System.DateTime();  //文件时间
        public static System.DateTime Time = new System.DateTime(); //记录时间

        public static string user_mess = "user.txt";       //用户信息

        public static string key = "jutengyouxiangongsi12345";       //密钥

        public static string apiKey = "";
        public static string secretKey = "";

        //数据库连接
        public static string sqlconn= "Database=juteng;Data Source=localhost;User Id=root;Password=;CharSet=utf8;port=3306";

        
    }
}
