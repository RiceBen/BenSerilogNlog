using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;

namespace BenSerilogNlog
{
    public class BusinessLogic
    {
        public void RunBusiness()
        {
            Log.Logger.ForContext(this.GetType()).Information("Starting up");

            Log.Logger.ForContext(this.GetType()).Error("This is a error log");
        }
    }
}
