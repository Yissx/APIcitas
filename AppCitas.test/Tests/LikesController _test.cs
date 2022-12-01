using AppCitas.Service.DTOs;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using AppCitas.test.Helper;
using Microsoft.IdentityModel.Logging;
using AppCitas.test.Helpers;

namespace AppCitas.test.Tests;

public class LikesController__test
{
    private string apiRoute = "api/likes";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUri;
    private string registeredObject;
    private HttpContent httpContent;
    public LikesController__test()
    {
        _client = Test.Instance.Client;
    }

    [Theory]
    [InlineData("NotFound", "Todd", "Pa$$w0rd", "juanito")]
    public async Task AddLike_NotFound(string statusCode, string username, string password, string userLiked)
    {
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        requestUri = $"{apiRoute}/" + userLiked;

        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;

        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }


    [Theory]
    [InlineData("BadRequest", "Todd", "Pa$$w0rd", "todd")]
    public async Task AddLike_BadRequest(string statusCode, string username, string password, string userLiked)
    {

        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/" + userLiked;

        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Mitchell", "Pa$$w0rd", "tanner")]
    public async Task AddLike_OK(string statusCode, string username, string password, string userLiked)
    {
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/" + userLiked;

        httpResponse = await _client.PostAsync(requestUri, httpContent);
        _client.DefaultRequestHeaders.Authorization = null;
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("BadRequest", "mitchell", "Pa$$w0rd", "tanner")]
    public async Task AddLike_BadRequest2(string statusCode, string username, string password, string userLiked)
    {
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);


        requestUri = $"{apiRoute}/" + userLiked;
        httpResponse = await _client.PostAsync(requestUri, httpContent);
        httpResponse = await _client.PostAsync(requestUri, httpContent);

        _client.DefaultRequestHeaders.Authorization = null;
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "mason", "Pa$$w0rd")]
    public async Task GetUserLikes_OK(string statusCode, string username, string password)
    {
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);



        requestUri = $"{apiRoute}" + "?predicate=likedBy";

        httpResponse = await _client.GetAsync(requestUri);
        _client.DefaultRequestHeaders.Authorization = null;
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }
    #region Privated methods
    private static string GetRegisterObject(RegisterDto registerDto)
    {
        var entityObject = new JObject()
            {
                { nameof(registerDto.Username), registerDto.Username },
                { nameof(registerDto.KnownAs), registerDto.KnownAs },
                { nameof(registerDto.Gender), registerDto.Gender },
                { nameof(registerDto.DateOfBirth), registerDto.DateOfBirth },
                { nameof(registerDto.City), registerDto.City },
                { nameof(registerDto.Country), registerDto.Country },
                { nameof(registerDto.Password), registerDto.Password }
            };

        return entityObject.ToString();
    }

    private static string GetRegisterObject(LoginDto loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
        return entityObject.ToString();
    }

    private StringContent GetHttpContent(string objectToEncode)
    {
        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
    }

    #endregion
}
