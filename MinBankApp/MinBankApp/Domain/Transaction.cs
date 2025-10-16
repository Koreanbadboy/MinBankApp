namespace MinBankApp.Domain;

public enum TransactionType
{
    Deposit,
    Withdrawal,
    TransferIn,
    TransferOut
}
public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public Guid? FromAccountId { get; set; }
    public Guid? ToAccountId { get; set; }

    // läsbar information
    public string FromAccountName { get; set; } = "";
    public string ToAccountName { get; set; } = "";

    public decimal Amount { get; set; }
    public string Description { get; set; } = "";

    public TransactionType TransactionType { get; set; }
}