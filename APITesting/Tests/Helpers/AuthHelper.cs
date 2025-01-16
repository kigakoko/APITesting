using APITesting.Framework;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace APITesting.Tests.Helpers;

public static class AuthHelper
{
    public static string GetAccessToken()
    {
			var url = ConfigManager.GetAuthSetting("TokenUrl");
			var client = new RestClient(ConfigManager.GetAuthSetting("TokenUrl"));
        var request = new RestRequest(url, Method.Post)
            .AddParameter("client_id", ConfigManager.GetAuthSetting("ClientId"))
            .AddParameter("client_secret", ConfigManager.GetAuthSetting("ClientSecret"))
            .AddParameter("scope", ConfigManager.GetAuthSetting("Scope"))
            .AddParameter("grant_type", "client_credentials");

        var response = client.ExecutePostAsync(request).Result;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            dynamic tokenResponse = JsonConvert.DeserializeObject(response.Content);
            return tokenResponse.access_token;
        }
        else
        {
            throw new Exception("Failed to retrieve access token");
        }
    }
}
