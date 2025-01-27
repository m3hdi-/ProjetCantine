using CantineKata.Domain.Enums;

namespace CantineKata.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal Solde { get; set; }
    public TypeClient TypeClient { get; set; }
}
