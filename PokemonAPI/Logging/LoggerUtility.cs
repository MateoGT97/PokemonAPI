
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace PokemonAPI.Logging;

public class LoggerUtility : ILoggerUtility
{
    public Serilog.ILogger Logger { get; set; } = null!;
    public string MainLogPath { get; set; } = null!;

    /// <summary>
    /// Creates a LogsUTC folder tree from the root directory given and initialize logging.
    /// </summary>
    /// <param name="rootFolder">Root directory of the folder where to create logs.</param>
    public void CreateLog(string rootFolder)
    {
        if (Logger == null)
        {
            DateTime utcNow = DateTime.UtcNow;
            MainLogPath = Path.Combine($@"LogsUTC\{utcNow.Year}\{utcNow.Month}\{utcNow.Day}\log.json");
            RollingInterval interval = RollingInterval.Hour;
            LogEventLevel level = LogEventLevel.Information;
            long fileSizeLimit = 100000000;
            int retainedFileCount = 24*7;
            Logger = new LoggerConfiguration()
                .Enrich.With(new TaskIDEnricher())
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .MinimumLevel.Information()
                .WriteTo.File(new CompactJsonFormatter(), MainLogPath, fileSizeLimitBytes: fileSizeLimit,
                    rollingInterval: interval, restrictedToMinimumLevel: level, retainedFileCountLimit: retainedFileCount)
                .CreateLogger();
        }
    }
    public void CloseAndFlush()
    {
        Log.CloseAndFlush();
    }

}