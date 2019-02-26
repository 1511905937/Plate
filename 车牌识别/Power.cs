using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace 车牌识别
{
    class Power
    {
        public static int power = 0;
        private string name = "";
        private string pwd = "";
        private string a_name = "root";
        private string a_pwd = "wangweili";

        public Power()
        {
            try
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
                this.name = mess[0];
                this.pwd = mess[1];
            }
            catch
            {

            }
        }
        public int login(string name,string pwd)
        {
            if (name==this.name&&pwd==this.pwd)
            {
                power = 1;
                return 1;
            }
            else if(name==a_name&&pwd==a_pwd)
            {
                power = 1;
                return 1;
            }
            return 0;
        }

        public int changepwd(string nname,string npwd,string oname,string opwd)
        {
            try
            {
                if (power == 1)
                {
                    
                    if ((oname == this.name && opwd == this.pwd)||(oname == this.a_name && opwd == this.a_pwd))
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
                        mess[0] = nname;
                        mess[1] = npwd;
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

                        return 1;
                    }
                }
            }
            catch
            {

            }
            return 0;
        }
    }
}
