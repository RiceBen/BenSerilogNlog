using System;
using Serilog;

namespace BenSerilogNlog
{
    class Program
    {
        static void Main(string[] args)
        {
            var myLogger = new MyLogger();

            myLogger.LogInfo("Information");

            Console.Read();
        }
    }
}
