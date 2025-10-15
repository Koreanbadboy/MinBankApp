using MinBankApp.Domain;
using MinBankApp.Interfaces;

namespace MinBankApp.Services;

public class TransactionService : ITransactionService
{
    private const string StorageKey = "transactions";
    private readonly IAccountService _accountService;
    private readonly IStorageService _storageService;
    private List<Transaction> _items = new();

    public TransactionService(IAccountService accountService, IStorageService storageService)
    {
        _accountService = accountService;
        _storageService = storageService;
    }

    private async Task LoadFromStorageAsync()
    {
        var loaded = await _storageService.GetItemAsync<List<Transaction>>(StorageKey);
        if (loaded != null)
            _items = loaded;
    }

    private async Task SaveToStorageAsync()
    {
        await _storageService.SetItemAsync(StorageKey, _items);
    }

    public async Task CreateAsync(Transaction tx)
    {
        await LoadFromStorageAsync();
        var accounts = await _accountService.GetAccounts();
        var account = accounts.FirstOrDefault(a => a.Name == tx.FromAccountName)
            ?? throw new InvalidOperationException($"Konto '{tx.FromAccountName}' hittades inte.");
        account.Withdraw(tx.Amount);
        tx.ToAccountName = tx.FromAccountName;
        _items.Add(tx);
        await SaveToStorageAsync();
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        await LoadFromStorageAsync();
        return _items.OrderByDescending(t => t.Date).ToList();
    }

    public async Task TransferAsync(string fromAccountName, string toAccountName, decimal amount, string? description = null)
    {
        await LoadFromStorageAsync();
        if (string.IsNullOrWhiteSpace(fromAccountName) || string.IsNullOrWhiteSpace(toAccountName))
            throw new ArgumentException("Från- och till-konto krävs.");
        if (fromAccountName == toAccountName)
            throw new InvalidOperationException("Kan inte föra över till samma konto.");
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Belopp måste vara > 0.");
        var accounts = await _accountService.GetAccounts();
        var from = accounts.FirstOrDefault(a => a.Name == fromAccountName)
                   ?? throw new InvalidOperationException($"Från-konto '{fromAccountName}' hittades inte.");
        var to = accounts.FirstOrDefault(a => a.Name == toAccountName)
                 ?? throw new InvalidOperationException($"Till-konto '{toAccountName}' hittades inte.");
        if (from.Currency == CurrencyType.None || to.Currency == CurrencyType.None)
            throw new InvalidOperationException("Ett eller båda konton har ingen giltig valuta inställd. Välj endast konton med en giltig valuta.");
        if (from.Currency != to.Currency)
            throw new InvalidOperationException($"Valutor matchar inte ({from.Currency} → {to.Currency}).");
        from.Withdraw(amount);
        to.Deposit(amount);
        var now = DateTime.Now;
        _items.Add(new Transaction {
            Date = now,
            FromAccountName = fromAccountName,
            ToAccountName = toAccountName,
            Amount = amount,
            Description = description ?? $"Transfer to {toAccountName}"
        });
        _items.Add(new Transaction {
            Date = now,
            FromAccountName = fromAccountName,
            ToAccountName = toAccountName,
            Amount = amount,
            Description = description ?? $"Transfer from {fromAccountName}"
        });
        await SaveToStorageAsync();
    }

    public async Task ClearAllAsync()
    {
        _items = new List<Transaction>();
        await SaveToStorageAsync();
    }
}
