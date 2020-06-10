using System;
using System.IO;

namespace SignalChat.Libs
{
    public class FileHelp
    {       
        public static void CreateFile(string path,string msg)
        {           
            if (File.Exists(path))
            {
                Console.WriteLine($"文件已经存在，请检查path值=>{path}");
                return;
            }
            else
            {
                Console.WriteLine("开始创建文件！");
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(msg);
                    }
                }                
            }            
        }
        public static void AppendToFile(string path, string msg)
        {
            Console.WriteLine("开始往文件里面添加信息");
            if (File.Exists(path))
            {
                using(TextWriter tw=new StreamWriter(path, true))
                {
                    tw.WriteLine(msg);
                }
            }
            else
            {
                Console.WriteLine($"文件不存在，请检查path值=>{path}");
            }
            return;
        }
    }
}
