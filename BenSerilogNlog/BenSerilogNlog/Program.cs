using System;
using Serilog;

namespace BenSerilogNlog
{
    class Program
    {
        static void Main(string[] args)
        {
            var myLogger = new MyLogger();

            myLogger.LogInfo("This Is An Information Message");

            Console.Read();
        }
    }
}
