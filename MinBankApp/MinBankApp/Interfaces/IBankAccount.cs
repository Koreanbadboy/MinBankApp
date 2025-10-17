namespace MinBankApp.Interfaces;

/// <summary>
/// Interface containing the BankAccount methods
/// </summary>
public interface IBankAccount
{
    Guid Id { get; set; }
    string Name { get; }
    public AccountType AccountType { get; }
    CurrencyType Currency { get; }
    decimal Balance { get;}
    DateTime LastUpdated { get; }
    List<Transaction> Transactions { get; }
    
    void Deposit(decimal amount, Guid fromAccountId, string fromAccountName, string description);
    void Withdraw(decimal amount, Guid toAccountId, string toAccountName, string description);
    //void TransferTo(BankAccount account, Guid toAccountId, string description, decimal amount); // arber
   
}