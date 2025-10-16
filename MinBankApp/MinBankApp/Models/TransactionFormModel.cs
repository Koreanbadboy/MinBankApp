using System.ComponentModel.DataAnnotations; // Innehåller attribut för validering av datamodeller, t.ex. [Required] och [Range]

namespace MinBankApp.Models;

public class TransactionFormModel
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Beloppet måste vara positivt.")]
    public decimal Amount { get; set; }

    [Required]
    public string Type { get; set; } = "Deposit"; // "Deposit" or "Withdraw"
}