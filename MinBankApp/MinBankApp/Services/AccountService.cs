public class AccountService : IAccountService
{
    private const string StorageKey = "bankapp.accounts";
    private readonly List<IBankAccount> _accounts;
    private readonly IStorageService _storageService;
    private bool isLoaded;

    public AccountService(IStorageService storageService)
    {
        _storageService = storageService;
        _accounts = new List<IBankAccount>();
    }

    private async Task IsInitialized()
    {
        if (isLoaded) return;
        var fromStorage = await _storageService.GetItemAsync<List<BankAccount>>(StorageKey);
        _accounts.Clear();
        if (fromStorage is { Count: > 0 })
            _accounts.AddRange(fromStorage);
        isLoaded = true;
    }

    private Task SaveAsync() => _storageService.SetItemAsync(StorageKey, _accounts);

    public async Task<IBankAccount> CreateAccount(string name, AccountType accountType, CurrencyType currency, decimal initialBalance)
    {
        await IsInitialized();
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        await SaveAsync();
        return account;
    }

    public async Task<List<IBankAccount>> GetAccounts()
    {
        await IsInitialized();
        return _accounts.Cast<IBankAccount>().ToList();
    }

    public async Task DeleteAccount(Guid accountId)
    {
        await IsInitialized();
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);
        if (account != null)
        {
            _accounts.Remove(account);
            await SaveAsync();
        }
    }

    public async Task UpdateAccount(BankAccount account)
    {
        await IsInitialized();
        var idx = _accounts.FindIndex(a => a.Id == account.Id);
        if (idx >= 0)
        {
            _accounts[idx] = account;
            await SaveAsync();
        }
    }
public async Task<IBankAccount?> GetAccountById(Guid accountId) // arber
    {
        await IsInitialized();
        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }

    public async void transfer(Guid fromAccountId, Guid toAccountId, decimal amount)
    {
        await IsInitialized();

        var fromAccount = _accounts.FirstOrDefault(a => a.Id == fromAccountId) as BankAccount;
        var toAccount = _accounts.FirstOrDefault(a => a.Id == toAccountId) as BankAccount;

        if (fromAccount == null || toAccount == null)
        {
            throw new InvalidOperationException("Ett eller båda kontona hittades inte.");
        }

        if (fromAccount.Balance < amount)
        {
            throw new InvalidOperationException("Inte tillräckligt med pengar på kontot.");
        }

        fromAccount.Withdraw(amount, toAccountId, toAccount.Name, $"Transfer to {toAccount.Name}");
        toAccount.Deposit(amount, fromAccountId, fromAccount.Name, $"Transfer from {fromAccount.Name}");

        await SaveAsync();
    }
}