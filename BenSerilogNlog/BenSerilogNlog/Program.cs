using System;
using Serilog;

namespace BenSerilogNlog
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
            logger.Information("Hello World");
            Console.Read();
        }
    }
}
