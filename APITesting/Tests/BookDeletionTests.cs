using RestSharp;
using FluentAssertions;
using APITesting.Framework;
using APITesting.Tests.Helpers;
using System.Net;
using APITesting.Tests.Models;
using Newtonsoft.Json;
using APITesting.Tests.Base;
using Serilog;

namespace APITesting.Tests;

[TestFixture]
public class BookDeletionTests : BaseTest
{
	[Test]
	public void DeleteBook_ValidId_ReturnsNoContent()
	{
		Log.Information("Starting to create a book for deletion test");

		var newBook = new Book
		{
			Title = "Delete Test",
			Author = "Suleka Slash",
			PublishedDate = "2014-01-15T16:38:11.792Z",
			Isbn = "978-0-18-345678-2"
		};

		var newBookResponse = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("CreateBook"), Method.Post, newBook);
		var newBookResponseData = JsonConvert.DeserializeObject<Book>(newBookResponse.Content);

		Log.Information($"Created book with ID: {newBookResponseData.Id}, now attempting to delete");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("DeleteBook"), Method.Delete, id: newBookResponseData.Id);
		response.StatusCode.Should().Be(HttpStatusCode.NoContent, "the response should indicate that the book was deleted successfully");

		Log.Information($"Deleted book with ID: {newBookResponseData.Id}");

		var getResponse = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("GetBook"), Method.Get, id: newBookResponseData.Id);
		getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "the response should indicate that the book was not found after deletion");

		Log.Information("Verified that the book is no longer retrievable");

		Test.Pass("Book deleted successfully and is no longer retrievable");
	}

	[Test]
	public void DeleteBook_NonExistentId_ReturnsNotFound()
	{
		var nonExistentBookId = "c0e362b6-f2f4-4642-9961-544a87f33a95";

		Log.Information($"Attempting to delete non-existent book with ID: {nonExistentBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("DeleteBook"), Method.Delete, id: nonExistentBookId);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound, "the response should indicate that the book was not found");

		Log.Information($"Handled non-existent book ID: {nonExistentBookId} correctly");

		Test.Pass("Handled non-existent book ID correctly");
	}

	[Test]
	public void DeleteBook_InvalidIdFormat_ReturnsBadRequest()
	{
		var invalidBookId = "invalid-book-id-format";

		Log.Information($"Attempting to delete book with invalid ID format: {invalidBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("DeleteBook"), Method.Delete, id: invalidBookId);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "the response should indicate a bad request due to invalid ID format");

		Log.Information($"Handled invalid book ID format: {invalidBookId} correctly");

		Test.Pass("Handled invalid book ID format correctly");
	}
}
