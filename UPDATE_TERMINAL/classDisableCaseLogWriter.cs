﻿using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace UPDATE_TERMINAL
{
    class classSendEmailLogWriter
    {
        public static FileStream fs;
        public static StreamWriter w;

        public classSendEmailLogWriter()
        {
            fs = null;
            w = null;
        }

        public static void OpenFileWriter()
        {
            string rootpath = Application.StartupPath;
            string filename = rootpath + "\\log\\" + "SendEmail" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            try
            {
                fs = new FileStream(filename, FileMode.Append);
                w = new StreamWriter(fs, Encoding.ASCII);
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                return;
            }
        }

        public static void CloseFileWriter()
        {
            if (w != null)
            {
                w.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }
        }

        public static void WriteLog(string content)
        {
            OpenFileWriter();
            content = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + content;
            w.WriteLine(content);
            w.Flush();
            CloseFileWriter();
        }
    }
}
