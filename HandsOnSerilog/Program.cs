using Serilog;

using var log = new LoggerConfiguration()
                .WriteTo.Console() // Sinks.Console
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day) // Sinks.File
                .CreateLogger();

log.Information("Hello, Serilog!");
log.Error("Something went wrong");
log.Debug("Debugging something");