using AppCitas.Service.DTOs;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using AppCitas.test.Helper;
using Windows.Storage;
using AppCitas.test.Helpers;

namespace AppCitas.test.Tests;

public class UsersController__test
{
    private string apiRoute = "api/users";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUri;
    private string registeredObject;
    private HttpContent httpContent;

    public UsersController__test()
    {
        _client = Test.Instance.Client;
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Theory]
    [InlineData("OK", "Karen", "Pa$$w0rd")]
    public async Task GetUsersNoPagination_OK(string statusCode, string username, string password)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;

        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Karen", "Pa$$w0rd", 1, 10)]
    public async Task GetUsersWithPagination_OK(string statusCode, string username, string password, int pageSize, int pageNumber)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;

        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        requestUri = $"{apiRoute}" + "?pageNumber=" + pageSize + "&pageSize" + pageNumber;

        // Act
        httpResponse = await _client.GetAsync(requestUri);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Todd", "Pa$$w0rd")]
    public async Task GetUserByUsername_OK(string statusCode, string username, string password)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        requestUri = $"{apiRoute}/" + username;

        // Act
        httpResponse = await _client.GetAsync(requestUri);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NoContent", "Lisa", "Pa$$w0rd", "IntroductionU", "LookingForU", "InterestsU", "CityU", "CountryU")]
    public async Task UpdateUser_NoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        var memberUpdateDto = new MemberUpdateDto
        {
            Introduction = introduction,
            LookingFor = lookingFor,
            Interests = interests,
            City = city,
            Country = country
        };
        registeredObject = GetRegisterObject(memberUpdateDto);
        httpContent = GetHttpContent(registeredObject);

        requestUri = $"{apiRoute}";

        // Act
        httpResponse = await _client.PutAsync(requestUri, httpContent);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Created", "Todd", "Pa$$w0rd", "../../Assets/random2.jpg")]
    public async Task AddPhoto_Created(string statusCode, string username, string password, string file)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        MultipartFormDataContent form = new MultipartFormDataContent();
        HttpContent content = new StringContent(file);
        form.Add(content, file);
        StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Console.Write(string.Format("args1: {0}", storageFolder));
        StorageFile sampleFile = await storageFolder.GetFileAsync(file);
        var stream = await sampleFile.OpenStreamForReadAsync();
        content = new StreamContent(stream);
        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "File",
            FileName = sampleFile.Name
        };
        form.Add(content);

        requestUri = $"{apiRoute}" + "/add-photo";

        // Act
        httpResponse = await _client.PostAsync(requestUri, form);

        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("Ok", "mason", "Pa$$w0rd", "../../Assets/random1.jpg")]
    public async Task SetMainPhoto_OK(string statusCode, string username, string password, string file)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        MultipartFormDataContent form = new MultipartFormDataContent();
        HttpContent content = new StringContent(file);
        form.Add(content, file);
        StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Console.Write(string.Format("args1: {0}", storageFolder));
        StorageFile sampleFile = await storageFolder.GetFileAsync(file);
        var stream = await sampleFile.OpenStreamForReadAsync();
        content = new StreamContent(stream);
        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "File",
            FileName = sampleFile.Name
        };
        form.Add(content);

        requestUri = $"{apiRoute}" + "/add-photo";

        // Act
        var result = await _client.PostAsync(requestUri, form);
        var messageJson = await result.Content.ReadAsStringAsync();
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];



        requestUri = $"{apiRoute}" + "/set-main-photo/" + id;

        // Act
        httpResponse = await _client.PutAsync(requestUri, null);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("OK", "Lisa", "Pa$$w0rd", "../../Assets/random3.jpg")]
    public async Task DeletePhoto_OK(string statusCode, string username, string password, string file)
    {
        // Arrange
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        MultipartFormDataContent form = new MultipartFormDataContent();
        HttpContent content = new StringContent(file);
        form.Add(content, file);
        StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Console.Write(string.Format("args1: {0}", storageFolder));
        StorageFile sampleFile = await storageFolder.GetFileAsync(file);
        var stream = await sampleFile.OpenStreamForReadAsync();
        content = new StreamContent(stream);
        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "File",
            FileName = sampleFile.Name
        };
        form.Add(content);

        requestUri = $"{apiRoute}" + "/add-photo";

        // Act
        var result = await _client.PostAsync(requestUri, form);
        var messageJson = await result.Content.ReadAsStringAsync();
        var message = messageJson.Split(',');
        var id = message[0].Split("\"")[2].Split(":")[1];


        requestUri = $"{apiRoute}" + "/delete-photo/" + id;

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    [Theory]
    [InlineData("NotFound", "Mitchell", "Pa$$w0rd", "20")]
    public async Task DeletePhoto_NotFound(string statusCode, string username, string password, string id)
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization = null;
        var user = await Login.LoginUser(username, password);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);



        requestUri = $"{apiRoute}" + "/delete-photo/" + id;

        // Act
        httpResponse = await _client.DeleteAsync(requestUri);
        // Assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
        Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods
    private static string GetRegisterObject(MemberUpdateDto memberUpdateDto)
    {
        var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
            };
        return entityObject.ToString();
    }
    private static string GetRegisterObject(string file)
    {
        var entityObject = new JObject()
            {
                { "File", file}
            };
        return entityObject.ToString();
    }
    private StringContent GetHttpContent(string objectToEncode)
    {
        return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
    }

    #endregion
}
