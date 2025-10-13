namespace MinBankApp.Interfaces;

public interface IStorageService
{
    //Spara
    Task SetItemAsync<T>(string key, T value);
    //Hämta
    Task<T?> GetItemAsync<T>(string key);
}