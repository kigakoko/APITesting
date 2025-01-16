using APITesting.Framework;
using APITesting.Tests.Base;
using APITesting.Tests.Helpers;
using APITesting.Tests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using RestSharp;
using System.Net;
using Serilog;

namespace APITesting.Tests;

[TestFixture]
[Parallelizable]
public class BookRetrievalTests : BaseTest
{
	[Test]
	public void GetAllBooks_ReturnsListOfBooks()
	{
		Log.Information("Starting test to retrieve all books");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("GetAllBooks"), Method.Get);
		response.StatusCode.Should().Be(HttpStatusCode.OK, "the response should be successful");

		var responseData = JsonConvert.DeserializeObject<List<Book>>(response.Content);
		responseData.Should().NotBeNull("the response should not be null");
		responseData.Should().NotBeEmpty("the response should contain at least one book");

		Log.Information("Successfully retrieved all books");

		Test.Pass("Retrieved all books successfully");
	}

	[Test]
	public void GetBookById_ValidId_ReturnsBook()
	{
		var validBookId = "a46f70f3-b0fc-4147-854d-61879ab87d83";

		Log.Information($"Starting test to retrieve book with ID: {validBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("GetBook"), Method.Get, id: validBookId);
		response.StatusCode.Should().Be(HttpStatusCode.OK, "the response should be successful");

		var responseData = JsonConvert.DeserializeObject<Book>(response.Content);
		responseData.Should().NotBeNull("the response should not be null");
		responseData.Id.Should().Be(validBookId, "the book ID in the response should match the requested ID");

		Log.Information($"Successfully retrieved book with ID: {validBookId}");

		Test.Pass("Retrieved book by valid ID successfully");
	}

	[Test]
	public void GetBookById_NonExistentId_ReturnsNotFound()
	{
		var nonExistentBookId = "c0e362b6-f2f4-4642-9961-544a87f33a95";

		Log.Information($"Attempting to retrieve book with non-existent ID: {nonExistentBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("GetBook"), Method.Get, id: nonExistentBookId);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound, "the response should indicate that the book was not found");

		Log.Information($"Handled non-existent book ID: {nonExistentBookId} correctly");

		Test.Pass("Handled non-existent book ID correctly");
	}

	[Test]
	public void GetBookById_InvalidIdFormat_ReturnsBadRequest()
	{
		var invalidBookId = "invalid-book-id-format";

		Log.Information($"Attempting to retrieve book with invalid ID format: {invalidBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("GetBook"), Method.Get, id: invalidBookId);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "the response should indicate a bad request due to invalid ID format");

		Log.Information($"Handled invalid book ID format: {invalidBookId} correctly");

		Test.Pass("Handled invalid book ID format correctly");
	}
}
