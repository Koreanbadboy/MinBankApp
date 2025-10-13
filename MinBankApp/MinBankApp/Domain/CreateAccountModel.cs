public class CreateAccountModel
{
    public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public CurrencyType Currency { get; set; }
    public decimal InitialBalance { get; set; }

    public void Clear()
    {
        Name = string.Empty;
        Type = default;
        Currency = default;
        InitialBalance = 0;
    }
}