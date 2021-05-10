using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
namespace UPDATE_TERMINAL
{
    class classUpdateTerminalLogWriter
    {
         public static FileStream fs;
        public static StreamWriter w;
        public classUpdateTerminalLogWriter()
        {
            fs = null;
            w = null;
        }
        public static void OpenFileWrite()
        {
            string rootpath = Application.StartupPath;
            string filename = rootpath + "\\Log\\" + "UpdateTerminal_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
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
        public static void CloseFileWrite()
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
            OpenFileWrite();           
            content = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + content;
            w.WriteLine(content);
            w.Flush();
            CloseFileWrite();

        }
    }
}
