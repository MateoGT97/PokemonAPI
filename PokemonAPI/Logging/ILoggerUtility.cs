namespace PokemonAPI.Logging;

public interface ILoggerUtility
{
    /// <summary>
    /// Main active logger
    /// </summary>
    public Serilog.ILogger Logger { get; set; }
    /// <summary>
    /// Path of the active log.
    /// </summary>
    public string MainLogPath { get; set; }

    /// <summary>
    /// Creates a LogsUTC folder tree from the root directory given and initialize logging.
    /// </summary>
    /// <param name="rootFolder">Root directory of the folder where to create logs.</param>
    public void CreateLog(string rootFolder);

    /// <summary>
    /// Close and flushes the active logger
    /// </summary>
    public void CloseAndFlush();
}