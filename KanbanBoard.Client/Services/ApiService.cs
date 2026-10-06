using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;

namespace KanbanBoard.Client.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ApiService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<AuthResponse?> LoginAsync(string login, string password)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", new { login, password });
        if (!response.IsSuccessStatusCode) return null;
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result?.Success == true && result.Token != null)
        {
            await _localStorage.SetItemAsync("token", result.Token);
            await _localStorage.SetItemAsync("login", result.Login);
            await _localStorage.SetItemAsync("role", result.Role);
        }
        return result;
    }

    public async Task<bool> RegisterAsync(string login, string password, string role = "Developer")
    {
        var response = await _http.PostAsJsonAsync("api/Auth/register",
            new { login, password, role });
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("token");
        await _localStorage.RemoveItemAsync("login");
        await _localStorage.RemoveItemAsync("role");
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<List<Project>?> GetProjectsAsync()
    {
        await AddAuthHeaderAsync();
        return await _http.GetFromJsonAsync<List<Project>>("api/Projects");
    }

    public async Task<bool> CreateProjectAsync(string name)
    {
        await AddAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync("api/Projects", new { name });
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Board>?> GetBoardsAsync(int projectId)
    {
        await AddAuthHeaderAsync();
        return await _http.GetFromJsonAsync<List<Board>>($"api/Boards/project/{projectId}");
    }

    public async Task<bool> CreateBoardAsync(string name, int projectId)
    {
        await AddAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync("api/Boards", new { name, projectId });
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Column>?> GetColumnsAsync(int boardId)
    {
        await AddAuthHeaderAsync();
        return await _http.GetFromJsonAsync<List<Column>>($"api/Boards/{boardId}/columns");
    }

    public async Task<bool> CreateColumnAsync(string name, int boardId, int? wipLimit = null)
    {
        await AddAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync("api/Boards/columns",
            new { name, boardId, wipLimit });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateCardAsync(string title, int columnId)
    {
        await AddAuthHeaderAsync();
        var response = await _http.PostAsJsonAsync("api/Cards",
            new { title, columnId, priority = "Medium" });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> MoveCardAsync(int cardId, int newColumnId)
    {
        await AddAuthHeaderAsync();
        var response = await _http.PutAsync($"api/Cards/{cardId}/move/{newColumnId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCardAsync(int cardId)
    {
        await AddAuthHeaderAsync();
        var response = await _http.DeleteAsync($"api/Cards/{cardId}");
        return response.IsSuccessStatusCode;
    }
}

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Login { get; set; }
    public string? Role { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int ProjectId { get; set; }
}

public class Column
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Order { get; set; }
    public int? WipLimit { get; set; }
    public int BoardId { get; set; }
    public List<Card> Cards { get; set; } = new();
}

public class Card
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Priority { get; set; } = "Medium";
    public int ColumnId { get; set; }
}