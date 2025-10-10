using System.Runtime.CompilerServices;
using MinBankApp.Domain;

namespace MinBankApp.Services;

public class AccountService : IAccountService
{
    private readonly List<IBankAccount> _accounts;

    public AccountService()
    {
        _accounts = new List<IBankAccount>();
    }
    public IBankAccount CreateAccount(string name, AccountType accountType, string currency, decimal initialBalance)
    {
        var account = new BankAccount(name, accountType, currency, initialBalance);
        _accounts.Add(account);
        return account;
    }

    public List<IBankAccount> GetAccounts()
    {
        return _accounts;
    }
}