using MinBankApp.Domain;

namespace MinBankApp.Interfaces;

public interface ITransactionService
{
    Task CreateAsync(Transaction tx);
    Task<List<Transaction>> GetAllAsync();
    
    Task TransferAsync(string fromAccountName, string toAccountName, decimal amount, string? description = null);
}