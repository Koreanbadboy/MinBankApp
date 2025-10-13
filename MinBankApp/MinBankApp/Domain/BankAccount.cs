using System.Text.Json.Serialization;
using MinBankApp.Interfaces;

namespace MinBankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } 
    public AccountType AccountType { get; private set; }
    public CurrencyType Currency { get; private set; } 
    public decimal Balance { get; private set; } 
    public DateTime LastUpdated { get; private set; } 

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
        DateTime lastUpdated)
    {
        Id = id;
        Name = name;
        AccountType = accountType;
        Balance = balance;
        Currency = currency;
        
        LastUpdated = lastUpdated;
    }
    
    
    /// <summary>
    /// Nedan är kalkyleringar för withdraw och deposit
    /// </summary>
    
    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));
        if (amount > Balance) throw new InvalidOperationException("Insufficient funds.");
        Balance -= amount;
        LastUpdated = DateTime.Now;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(amount));
        Balance += amount;
        LastUpdated = DateTime.Now;
    }
}