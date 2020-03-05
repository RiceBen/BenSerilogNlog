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
            var position = new { Latitude = 25, Longitude = 134 };
            var fruit = new Dictionary<string, int> { { "Apple", 1 }, { "Pear", 5 } };

            Log.Logger.ForContext(this.GetType()).Information("Starting up");
            Log.Logger.ForContext(this.GetType()).Information("In my bowl I have {Fruit}", fruit);
            Log.Logger.ForContext(this.GetType()).Warning("Processed {@Position}", position);
            Log.Logger.ForContext(this.GetType()).Error("This is a error log");
        }
    }
}
