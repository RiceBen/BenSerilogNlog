using System;
using Amazon;
using Amazon.CloudWatchLogs;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting;
using Serilog.Sinks.AwsCloudWatch;

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
                            .WriteTo.Logger(AwsCloudwatchSinkConfiger)
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

        private static void AwsCloudwatchSinkConfiger(LoggerConfiguration lscf)
        {
            // name of the log group
            var logGroupName = "myLogGroup/dev";
            
            // options for the sink defaults in https://github.com/Cimpress-MCP/serilog-sinks-awscloudwatch/blob/master/src/Serilog.Sinks.AwsCloudWatch/CloudWatchSinkOptions.cs
            var options = new CloudWatchSinkOptions
            {
                // you must provide a text formatter, otherwise exception throw
                TextFormatter = new Serilog.Formatting.Json.JsonFormatter(),
                // the name of the CloudWatch Log group for logging
                LogGroupName = logGroupName,
                
                // other defaults defaults
                MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                BatchSizeLimit = 100,
                QueueSizeLimit = 10000,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 5
            };
            
            // setup AWS CloudWatch client
            var client = new AmazonCloudWatchLogsClient("yourAwsAccessKey", "yourAwsSecretAccessKey", RegionEndpoint.USWest2);

            lscf.WriteTo
                .AmazonCloudWatch(options, client);
        }
    }
}