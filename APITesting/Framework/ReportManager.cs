using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace APITesting.Framework;

public static class ReportManager
{
	private readonly static ExtentReports _extent;
	private readonly static ExtentSparkReporter _htmlReporter;

	private static readonly string BaseDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..");

	static ReportManager()
	{
		var reportDirectory = GetReportDirectory();

		_htmlReporter = new ExtentSparkReporter(Path.Combine(reportDirectory, "TestExecutionReport.html"));
		_extent = new ExtentReports();
		_extent.AttachReporter(_htmlReporter);
	}

	public static ExtentTest CreateTest(string testName)
	{
		return _extent.CreateTest(testName);
	}

	public static void FlushReports()
	{
		_extent.Flush();
	}

	private static string GetReportDirectory()
	{
		var reportDirectory = Path.Combine(BaseDirectory, "Reports");

		if (!Directory.Exists(reportDirectory))
		{
			Directory.CreateDirectory(reportDirectory);
		}

		return reportDirectory;
	}
}
