using MinBankApp.Domain;

namespace MinBankApp.Interfaces;

public interface IAccountService
{
    Task<CreateAccountModel> LoadModelAsync();
    Task SaveModelAsync(CreateAccountModel model);
    Task<IBankAccount> CreateAccount(string name, AccountType accountType, CurrencyType currency, decimal initialBalance);
    Task<List<IBankAccount>> GetAccounts();
    
}