using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubular.Logging
{
    public static class Logger
    {
        static object consoleLockObj = new object();

        public static void Log(string str, ConsoleColor color)
        {
            lock (consoleLockObj)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(str);
                Console.ResetColor();
            }
        }
    }
}
