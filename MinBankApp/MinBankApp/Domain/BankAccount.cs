using System.Text.Json.Serialization;
using MinBankApp.Interfaces;

namespace MinBankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public AccountType AccountType { get; private set; }
    public CurrencyType Currency { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime LastUpdated { get; private set; }
    private List<Transaction> _transactions = new List<Transaction>();

    public List<Transaction> Transactions => _transactions;

    public BankAccount(string name, AccountType accountType, CurrencyType currency, decimal initialBalance)
    {
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
    }

    [JsonConstructor]
    public BankAccount(Guid id, string name, AccountType accountType, CurrencyType currency, decimal balance,
        DateTime lastUpdated, List<Transaction> transactions)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Balance = balance;
        Currency = currency;
        LastUpdated = lastUpdated;
        _transactions = transactions; // Lägg till transaktioner vid återskapande
    }

    /// <summary>
    /// Drar pengar från kontot och skapar en transaktion för uttaget.
    /// </summary>
    public void Withdraw(decimal amount, Guid toAccountId, string toAccountName, string description)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));
        if (amount > Balance) throw new InvalidOperationException("Insufficient funds.");
        
        Balance -= amount;
        LastUpdated = DateTime.Now;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            TimeStamp = DateTime.Now,
            TransactionType = TransactionType.Withdrawal,
            FromAccountId = this.Id,
            FromAccountName = this.Name,
            ToAccountId = toAccountId,
            ToAccountName = toAccountName,
            Description = description
        };
        _transactions.Add(transaction);
    }

    /// <summary>
    /// Sätter in pengar på kontot och skapar en transaktion för insättningen.
    /// </summary>
    public void Deposit(decimal amount, Guid fromAccountId, string fromAccountName, string description)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));
        
        Balance += amount;
        LastUpdated = DateTime.Now;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            TimeStamp = DateTime.Now,
            TransactionType = TransactionType.Deposit,
            FromAccountId = fromAccountId,
            FromAccountName = fromAccountName,
            ToAccountId = this.Id,
            ToAccountName = this.Name,
            Description = description
        };
        _transactions.Add(transaction);
    }
}
