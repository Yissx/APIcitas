using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AppCitas.Service.DTOs;
using AppCitas.test.Helper;
using AppCitas.test.Helpers;
using Newtonsoft.Json.Linq;

namespace AppCitas.test.Tests;

public class MessagesController__test
{
    private string apiRoute = "api/messages";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUri;
    private string registeredObject;
    private HttpContent httpContent;
    public MessagesController__test()
    {
        _client = Test.Instance.Client;
    }
    [Theory]
    [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa", "Hola")]
    public async Task CreateMessage_BadRequest(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessagesDto
        {
            RecipientUsername = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    [Theory]
    [InlineData("NotFound", "rosa", "Pa$$w0rd", "amadocruz", "Hola")]
    public async Task CreateMessage_NotFound(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessagesDto
        {
            RecipientUsername = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "lisa", "Hola")]
    public async Task CreateMessage_OK(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessagesDto
        {
            RecipientUsername = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "lisa", "Pa$$w0rd")]
    public async Task GetMessagesForUser_OK(string statusCode, string username, string password)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "mason", "Pa$$w0rd", "Outbox")]
    public async Task GetMessagesForUserFromQuery_OK(string statusCode, string username, string password, string container)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}" + "?container=" + container;

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "kristin")]
    public async Task GetMessagesThread_OK(string statusCode, string username, string password, string user2)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}/thread/" + user2;

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    [Theory]
    [InlineData("OK", "todd", "Pa$$w0rd", "mitchell", "Hola")]
    public async Task DeleteMessage_OK(string statusCode, string username, string password, string recipientUsername, string content)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessagesDto
        {
            RecipientUsername = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";
        var result = await _client.PostAsync(requestUri, httpContent);
        var messageJson = await result.Content.ReadAsStringAsync();
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];
        requestUri = $"{apiRoute}/" + id;

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        user = await Login.LoginUser(recipientUsername, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Unauthorized", "todd", "Pa$$w0rd", "lisa", "Hola", "gonzalez")]
    public async Task DeleteMessage_Unauthorized(string statusCode, string username, string password, string recipientUsername, string content, string unauth)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var messageDto = new MessagesDto
        {
            RecipientUsername = recipientUsername,
            Content = content
        };
        registeredObject = GetRegisterObject(messageDto);
        httpContent = GetHttpContent(registeredObject);
        requestUri = $"{apiRoute}";
        var result = await _client.PostAsync(requestUri, httpContent);
        var messageJson = await result.Content.ReadAsStringAsync();
        _client.DefaultRequestHeaders.Authorization = null;
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];
        requestUri = $"{apiRoute}/" + id;

        user = await Login.LoginUser(unauth, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    #region Privated methods
    private static string GetRegisterObject(MessagesDto message)
    {
        var entityObject = new JObject()
            {
                { nameof(message.RecipientUsername), message.RecipientUsername },
                { nameof(message.Content), message.Content }
            };
        return entityObject.ToString();
    }

    private StringContent GetHttpContent(string objectToEncode)
    {
        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
    }

    #endregion
}
