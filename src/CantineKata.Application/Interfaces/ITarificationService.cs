using CantineKata.Domain.Entities;

namespace CantineKata.Application.Interfaces
{
    public interface ITarificationService
    {
        decimal CalculTotalSupplement(List<string> supplements, out List<string> produits);
        decimal CalculerPriseEnchange(Client client, decimal montantTotal);
        bool VerifierSoldeSuffisant(Client client, decimal montantPayer);
    }
}
