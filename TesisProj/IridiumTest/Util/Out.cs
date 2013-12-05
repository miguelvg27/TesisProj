using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IridiumTest.Util
{
    public static class Out
    {
        public static void  WriteLineFile(String path, bool console, string text)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                if(console)
                        Console.WriteLine(text);
                sw.WriteLine(text);
            }
        }

        public static void WriteFile(String path, bool console, string text)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                if (console)
                    Console.Write(text);
                sw.Write(text);
            }
        }
    }
}
