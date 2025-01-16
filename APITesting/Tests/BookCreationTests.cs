using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using APITesting.Framework;
using APITesting.Tests.Helpers;
using System.Net;
using APITesting.Tests.Models;
using APITesting.Tests.Base;
using Serilog;

namespace APITesting.Tests;

[TestFixture]
[Parallelizable]
public class BookCreationTests : BaseTest
{
	private string? createdBookId;

	[Test]
	public void CreateBook_ValidBook_Returns201()
	{
		Log.Information("Creating a new book with valid data");

		var newBook = new Book
		{
			Title = "Amazing sssuper цщц mega Test",
			Author = "Suleka sSlash",
			PublishedDate = "2014-01-15T16:38:11.792Z",
			Isbn = "978-0-18-345678-2"
		};

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("CreateBook"), Method.Post, newBook);
		var responseData = JsonConvert.DeserializeObject<Book>(response.Content);

		response.StatusCode.Should().Be(HttpStatusCode.Created, "a valid book should result in a 201 status code");
		responseData.Title.Should().Be(newBook.Title, "the title in the response should match the input");
		responseData.Author.Should().Be(newBook.Author, "the author in the response should match the input");

		if (responseData?.Id != null)
		{
			createdBookId = responseData.Id;
		}

		Log.Information($"Book with ID {createdBookId} created successfully");

		Test.Pass("Book created successfully with valid data");
	}

	[Test]
	public void CreateBook_DuplicateBook_ReturnsError()
	{
		Log.Information("Creating a duplicate book");

		var duplicateBook = new Book
		{
			Title = "Amazing super mega Test",
			Author = "Sula Slash",
			PublishedDate = "2017-01-15T16:38:11.792Z",
			Isbn = "978-0-12-345678-2"
		};

		ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("CreateBook"), Method.Post, duplicateBook);

		var response = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("CreateBook"), Method.Post, duplicateBook);

		response.StatusCode.Should().Be(HttpStatusCode.Conflict, "creating a duplicate book should result in a 409 status code");

		Log.Information("Duplicate book creation handled correctly");

		Test.Pass("Duplicate book creation is handled correctly");
	}

	[TearDown]
	public void TearDown()
	{
		if (!string.IsNullOrEmpty(createdBookId))
		{
			Log.Information($"Deleting the book with ID {createdBookId}");

			var deleteResponse = ApiHelper.ExecuteRequest(ConfigManager.GetEndpoint("DeleteBook"), Method.Delete, id: createdBookId);
			deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent, "the book should be deleted successfully");

			Log.Information($"Book with ID {createdBookId} deleted successfully");

			Test.Pass("Created book deleted successfully");
		}
	}
}
