using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubular.Logging
{
    public struct LoggerClass
    {
        string className { get; set; }

        public LoggerClass(string className)
        {
            this.className = className;
        }

        public void Log(string str, ConsoleColor color)
        {
            Logger.Log("[" + className + "] " + str, color);
        }

        public void LogInfo(string str)
        {
            Logger.Log("[" + className + "] INFO: " + str, ConsoleColor.Gray);
        }

        public void LogWarning(string str)
        {
            Logger.Log("[" + className + "] WARN: " + str, ConsoleColor.Yellow);
        }

        public void LogError(string str)
        {
            Logger.Log("[" + className + "] ERROR: " + str, ConsoleColor.Red);
        }
    }
}
