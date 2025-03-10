using Serilog.Core;
using Serilog.Events;

namespace PokemonAPI.Logging;

/// <summary>
/// Custom enricher for Serilog that resgisters the executing Task Id
/// </summary>
public class TaskIDEnricher : ILogEventEnricher
{
    /// <summary>
    /// Enriches the current log entry with a property containing the current task id.
    /// </summary>
    /// <param name="logEvent">Log event to enrich</param>
    /// <param name="propertyFactory">Factory for creating ew properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(new LogEventProperty("TaskId", new ScalarValue(Task.CurrentId)));
    }
}