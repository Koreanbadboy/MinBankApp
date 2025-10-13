using System.Text.Json;
using MinBankApp.Interfaces;
using Microsoft.JSInterop;

namespace MinBankApp.Services;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public StorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
}