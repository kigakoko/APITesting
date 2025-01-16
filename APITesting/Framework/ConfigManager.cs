using Microsoft.Extensions.Configuration;

namespace APITesting.Framework;

public static class ConfigManager
{
	public static IConfiguration Configuration { get; }

	static ConfigManager()
	{
		Configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("Tests/Config/appsettings.json")
			.Build();
	}

	public static string GetBaseUrl() => Configuration["BaseUrl"];
	public static string GetEndpoint(string key) => Configuration[$"Endpoints:{key}"];
	public static string GetAuthSetting(string key) => Configuration[$"Authentication:{key}"];
}
