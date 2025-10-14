namespace MinBankApp.Domain;

public class Transaction
{
    public DateTime Date { get; set; } = DateTime.Today;
    public string FromAccountName { get; set; } = "";
    public string ToAccountName { get; set; } = "";
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
}