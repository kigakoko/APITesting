using APITesting.Framework;
using APITesting.Tests.Base;
using APITesting.Tests.Helpers;
using APITesting.Tests.Models;
using FluentAssertions;
using RestSharp;
using System.Net;
using Serilog;

namespace APITesting.Tests;

[TestFixture]
[Parallelizable]
public class BookUpdateTests : BaseTest
{
	[Test]
	public void UpdateBook_ValidData_ReturnsUpdatedBook()
	{
		var existingBookId = "a46f70f3-b0fc-4147-854d-61879ab87d83";

		var updatedBook = new Book
		{
			Title = "Updated Book Title",
			Author = "Updated Author",
			PublishedDate = "2024-01-15T16:38:11.792Z",
			Isbn = "978-0-18-345678-3"
		};

		Log.Information($"Starting test: Update book with ID {existingBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("UpdateBook"), Method.Put, updatedBook, id: existingBookId);
		response.StatusCode.Should().Be(HttpStatusCode.NoContent, "the response should indicate that the book was updated successfully");

		Log.Information("Book updated successfully");

		Test.Pass("Book updated successfully with valid data");
	}

	[Test]
	public void UpdateBook_NonExistentId_ReturnsNotFound()
	{
		var nonExistentBookId = "c0e362b6-f2f4-4642-9961-544a87f33a95";

		var updatedBook = new Book
		{
			Title = "Updated Book Title",
			Author = "Updated Author",
			PublishedDate = "2024-01-15T16:38:11.792Z",
			Isbn = "978-0-18-345678-3"
		};

		Log.Information($"Attempting to update book with non-existent ID: {nonExistentBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("UpdateBook"), Method.Put, updatedBook, id: nonExistentBookId);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound, "the response should indicate that the book was not found");

		Log.Information("Handled non-existent book ID correctly");

		Test.Pass("Handled non-existent book ID correctly");
	}

	[Test]
	public void UpdateBook_InvalidIdFormat_ReturnsBadRequest()
	{
		var invalidBookId = "invalid-book-id-format";

		var updatedBook = new Book
		{
			Title = "Updated Book Title",
			Author = "Updated Author",
			PublishedDate = "2024-01-15T16:38:11.792Z",
			Isbn = "978-0-18-345678-3"
		};

		Log.Information($"Attempting to update book with invalid ID format: {invalidBookId}");

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("UpdateBook"), Method.Put, updatedBook, id: invalidBookId);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "the response should indicate a bad request due to invalid ID format");

		Log.Information("Handled invalid book ID format correctly");

		Test.Pass("Handled invalid book ID format correctly");
	}
}
