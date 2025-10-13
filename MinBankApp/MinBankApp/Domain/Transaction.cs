namespace MinBankApp.Domain;

public class Transaction
{
    public DateTime Date { get; set; } = DateTime.Today;
    public string AccountName { get; set; } = "";   
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
}