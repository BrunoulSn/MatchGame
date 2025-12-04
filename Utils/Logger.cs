using Serilog;

namespace BFF_GameMatch.Utils
{
    public static class Logger
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/bff-log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
