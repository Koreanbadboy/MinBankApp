using System.Linq;
using MinBankApp.Domain;
using MinBankApp.Interfaces;

namespace MinBankApp.Services;

public class InMemoryTransactionService : ITransactionService
{
    private static readonly List<Transaction> _items = new();
    private readonly IAccountService _accountService;
    
    

    // Viktigt: DI injicerar IAccountService här. Skapa inte med "new" själv.
    public InMemoryTransactionService(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // Vanlig betalning (tolkas som utgift)
    public async Task CreateAsync(Transaction tx)
    {
        var accounts = await _accountService.GetAccounts();
        var account = accounts.FirstOrDefault(a => a.Name == tx.FromAccountName)
            ?? throw new InvalidOperationException($"Konto '{tx.FromAccountName}' hittades inte.");

        account.Withdraw(tx.Amount);

        // Set both FromAccountName and ToAccountName to the same for single-account transactions
        tx.ToAccountName = tx.FromAccountName;

        _items.Add(tx);
    }

    public Task<List<Transaction>> GetAllAsync()
    {
        return Task.FromResult(_items.OrderByDescending(t => t.Date).ToList());
    }

    // Överföring mellan två konton
    public async Task TransferAsync(string fromAccountName, string toAccountName, decimal amount, string? description = null)
    {
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

        // Enkelt läge: samma valuta krävs
        if (from.Currency != to.Currency)
            throw new InvalidOperationException($"Valutor matchar inte ({from.Currency} → {to.Currency}).");

        // Gör själva överföringen via domänmetoder
        from.Withdraw(amount);
        to.Deposit(amount);

        var now = DateTime.Now;

        // Logga två transaktioner (ut + in) för historik, with correct from/to
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
    }
}
