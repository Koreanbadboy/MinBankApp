using MinBankApp.Interfaces;

namespace MinBankApp.Domain;

public class BankAccount : IBankAccount
{
    public Guid Id { get; }
    public string Name { get; }
    public string Currency { get; }
    public decimal Balance { get; }
    public DateTime LastUpdated { get; }
    public void Withdraw(decimal amount)
    {
        throw new NotImplementedException();
    }

    public void Deposit(decimal amount)
    {
        throw new NotImplementedException();
    }
}