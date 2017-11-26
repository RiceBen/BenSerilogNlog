using System;
using Serilog;

namespace BenSerilogNlog
{
    public class MyLogger
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BenSerilogNlog.MyLogger"/> class.
        /// </summary>
        public MyLogger()
        {
            this._logger = new LoggerConfiguration()                 .WriteTo.Console(                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"                 )                 .CreateLogger();
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="message">Message.</param>
        public void LogInfo(string message){
            this._logger.Information(message);
        }
    }
}
