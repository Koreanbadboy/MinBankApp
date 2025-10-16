using System.ComponentModel.DataAnnotations;
using MinBankApp.Services;

namespace MinBankApp.Models;

public class CreateAccountFormModel
{
    [Required(ErrorMessage = "Kontonamn är obligatoriskt.")]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig kontotyp.")]
    public AccountType Type { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig valuta.")]
    public CurrencyType Currency { get; set; }
    
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Startsaldo får inte vara negativt.")]

    public decimal InitialBalance { get; set; }
}