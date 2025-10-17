using MinBankApp.Domain;

namespace MinBankApp.Interfaces;

public interface IAccountService
{
    Task<IBankAccount> CreateAccount(string name, AccountType accountType, CurrencyType currency, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    Task DeleteAccount(Guid accountId);
    Task UpdateAccount(BankAccount account);
    Task<IBankAccount?> GetAccountById(Guid accountId);
    
    void transfer(Guid fromAccountId, Guid toAccountId, decimal amount); // arber
    
}