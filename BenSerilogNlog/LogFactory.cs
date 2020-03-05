using Serilog;
using Serilog.Filters;

namespace BenSerilogNlog
{
    public class LogFactory
    {
        public static void InitMyLogger()
        {
            var isBSLN = Matching.FromSource<BusinessLogic>();
            var expr = "@Level = 'Information' and AppId3 is not null";

            Log.Logger = new LoggerConfiguration()
                            .Enrich.WithProperty("AppId", new { Id = 1, Name = "Ben" })
                            .WriteTo.Console(
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                outputTemplate: "{Timestamp:HH:mm:ss} [AppId:{AppId} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.NLog(Serilog.Events.LogEventLevel.Error, "[AppId:{AppId}] {Message:lj}{NewLine}{Exception}")
                            .WriteTo.Logger(logger =>
                                logger.Enrich.WithProperty("AppId2", 10)
                                      .Filter
                                      .ByIncludingOnly(isBSLN)
                                      .WriteTo.Console(Serilog.Events.LogEventLevel.Warning, "{Timestamp:HH:mm:ss} [AppId:{AppId2}{AppId} {Level:u3}]$ {Message:lj}{NewLine}{Exception}"))
                            .WriteTo.Logger(logger=>
                                logger.Enrich.WithProperty("AppId3", "[Terra]")
                                      .Filter
                                      .ByIncludingOnly(expr)
                                      .WriteTo.Console(
                                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                        outputTemplate: "{Timestamp:HH:mm:ss} [AppId:{AppId3}{AppId2}{AppId} {Level:u3}]$ {Message:lj}{NewLine}{Exception}"))
                            .CreateLogger();
        }
    }
}