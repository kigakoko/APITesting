using Serilog;

namespace APITesting.Framework;

public static class LoggerSetup
{
	public static void ConfigureLogging()
	{
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {ThreadId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
			.WriteTo.File(Path.Combine(Path.Combine(
				Directory.GetCurrentDirectory(), @"..\..\..\.."), "logs/log-.txt"),
				rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5
			)
			.MinimumLevel.Information()
			.CreateLogger();
	}
}
