using APITesting.Framework;
using AventStack.ExtentReports;

namespace APITesting.Tests.Base;

[SetUpFixture]
public class BaseTest
{
	protected ExtentTest Test;

	[OneTimeSetUp]
	public void GlobalSetup()
	{
		LoggerSetup.ConfigureLogging();
		string fullClassName = TestContext.CurrentContext.Test.ClassName!;
		string className = fullClassName[(fullClassName.LastIndexOf('.') + 1)..];
		Test = ReportManager.CreateTest(className);
	}

	[OneTimeTearDown]
	public void GlobalTeardown()
	{
		ReportManager.FlushReports();
	}
}
