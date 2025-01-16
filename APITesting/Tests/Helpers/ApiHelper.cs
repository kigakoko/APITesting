using APITesting.Framework;
using RestSharp;

namespace APITesting.Tests.Helpers;

public static class ApiHelper
{
	private static readonly RestClient _client = new(ConfigManager.GetBaseUrl());

	public static RestResponse ExecuteRequest(string endpoint, Method method, object body = null!, string id = null!)
	{
		var request = new RestRequest(endpoint, method);

		if (!string.IsNullOrEmpty(id))
		{
			request.AddUrlSegment("id", id);
		}

		var token = AuthHelper.GetAccessToken();
		request.AddHeader("Authorization", $"Bearer {token}");

		if (body != null)
		{
			request.AddJsonBody(body);
		}

		return _client.Execute(request);
	}
}
