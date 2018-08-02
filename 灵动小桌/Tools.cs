using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace 灵动小桌
{
    class Tools
    {
        
        /// <summary>
        /// 以下是配置文件相关
        /// </summary>
        public static class WinProfile
        {

            // 配置文件为当前目录下的config.ini文件
            public static readonly string CONFIG_FILE_PATH = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

            [DllImport("kernel32")] // 写入配置文件的接口
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32")] // 读取配置文件的接口
            private static extern int GetPrivateProfileString(string section, string key, string def,
            StringBuilder retVal, int size, string filePath);
            // 向配置文件写入值
            public static void ProfileWriteValue(string section, string key, string value, string path)
            {
                WritePrivateProfileString(section, key, value, path);
            }
            // 读取配置文件的值
            public static string ProfileReadValue(string section, string key, string path)
            {
                StringBuilder sb = new StringBuilder(255);
                GetPrivateProfileString(section, key, "", sb, 255, path);
                return sb.ToString().Trim();
            }
        }
        
        /**
         * 使用范例
         */
        //配置文件路径
        //string configpath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        //判断是否存在该文件
        //if (!File.Exists(configpath))
        //{
             // 不存在则创建
        //   FileStream fs = new FileStream(configpath, FileMode.OpenOrCreate);
        //}

        ////写入配置
        //WinProfile.ProfileWriteValue("Setting", "DefaultSerialPort", ssp.SL_PortName, configpath);
        ////读取配置
        //WinProfile.ProfileReadValue("Setting", "DefaultSerialPort", configpath);

        

    }
}
