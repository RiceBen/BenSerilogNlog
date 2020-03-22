using Serilog;

using Serilog.Filters;

namespace BenSerilogNlog
{
    public class LogFactory
    {
        public static void InitMyLogger()
        {

            Log.Logger = new LoggerConfiguration()
                            .Enrich.WithProperty("AppId", new { Id = 1, Name = "Ben" })
                            .WriteTo.Logger(MyDefaultConsoleSinkConfiger)
                            .WriteTo.Logger(MyDefaultNLogSinkConfiger)
                            .WriteTo.Logger(ConsoleLogWithPropAndFilterWithClassSinkConfiger)
                            .WriteTo.Logger(ConsoleLogWithPropAndFilterWithRegexSinkConfiger)
                            .CreateLogger();
        }

        private static void MyDefaultNLogSinkConfiger(LoggerConfiguration lscf)
        {
            lscf.WriteTo
                .NLog(Serilog.Events.LogEventLevel.Error, "[AppId:{AppId}] {Message:lj}{NewLine}{Exception}");
        }

        private static void MyDefaultConsoleSinkConfiger(LoggerConfiguration lscf)
        {
            lscf.WriteTo.Console(
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:HH:mm:ss} [AppId:{AppId} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        }

        private static void ConsoleLogWithPropAndFilterWithClassSinkConfiger(LoggerConfiguration lscf)
        {
            var isBSLN = Matching.FromSource<BusinessLogic>();

            lscf.Enrich
                .WithProperty("AppId2", 10)
                .Filter
                .ByIncludingOnly(isBSLN)
                .WriteTo.Console(
                    Serilog.Events.LogEventLevel.Warning,
                    "{Timestamp:HH:mm:ss} [AppId:{AppId2}{AppId} {Level:u3}]$ {Message:lj}{NewLine}{Exception}");
        }

        private static void ConsoleLogWithPropAndFilterWithRegexSinkConfiger(LoggerConfiguration lscf)
        {
            var expr = "@Level = 'Information' and AppId3 is not null";

            lscf.Enrich
                .WithProperty("AppId3", "[MyService]")
                .Filter
                .ByIncludingOnly(expr)
                .WriteTo.Console(
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    outputTemplate: "{Timestamp:HH:mm:ss} [AppId:{AppId3}{AppId2}{AppId} {Level:u3}]$ {Message:lj}{NewLine}{Exception}");
        }
    }
}