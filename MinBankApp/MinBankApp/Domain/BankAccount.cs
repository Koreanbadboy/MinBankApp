using MinBankApp.Interfaces;

namespace MinBankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } 
    public AccountType AccountType { get; private set; }
    public string Currency { get; private set; } 
    public decimal Balance { get; private set; } 
    public DateTime LastUpdated { get; private set; } 

    public BankAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        Name = name;
        AccountType = accountType;
        Currency = currency;
        Balance = initialBalance;
        LastUpdated = DateTime.Now;
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