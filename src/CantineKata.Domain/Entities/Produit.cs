using CantineKata.Domain.Enums;

namespace CantineKata.Domain.Entities;

public class Produit
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public TypeProduit TypeProduit { get; set; }
}