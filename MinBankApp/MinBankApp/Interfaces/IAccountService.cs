namespace MinBankApp.Interfaces;

public interface IAccountService
{
    IBankAccount CreateAccount(string name, string currency, decimal initialBalance);
    List<IBankAccount> GetAccounts();
}