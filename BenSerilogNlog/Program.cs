using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace BenSerilogNlog
{
    public static class Program
    {
        static void Main(string[] args)
        {
            LogFactory.InitMyLogger();

            var logic = new BusinessLogic();
            logic.RunBusiness();
            Log.Debug("this is debug message");
            Console.Read();
        }
    }
}
