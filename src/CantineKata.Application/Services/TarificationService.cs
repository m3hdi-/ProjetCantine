using CantineKata.Application.Interfaces;
using CantineKata.Domain.Entities;
using CantineKata.Domain.Enums;

namespace CantineKata.Application.Services
{
    public class TarificationService : ITarificationService
    {
        private readonly Dictionary<string, decimal> _supplements = new()
        {
            {"Boisson", 1m},
            {"Fromage", 1m},
            {"Pain", 0.4m},
            {"Petit Salade Bar", 4m},
            {"Grand Salade Bar", 6m},
            {"Portion de fruit", 1m},
            {"Entrée supplementaire", 3m},
            {"Plat supplementaire", 6m},
            {"Dessert supplementaire", 3m}
        };

        public decimal CalculerPriseEnchange(Client client, decimal montantTotal)
        {
            return client.TypeClient switch
            {
                TypeClient.Interne => 7.5m,
                TypeClient.Prestataire => 6m,
                TypeClient.Vip => montantTotal,
                TypeClient.Stagiaire => 10m,
                TypeClient.Visiteur => 0m,
                _ => 0m
            };
        }

        public bool VerifierSoldeSuffisant(Client client, decimal montantPayer)
        {
            if(client.TypeClient != TypeClient.Interne && client.TypeClient != TypeClient.Vip)
            {
                if(client.Solde < montantPayer)
                {
                    return false;
                }
            }
            return true;
        }

        public decimal CalculTotalSupplement(List<string> supplements, out List<string> produits)
        {
            decimal total = 10m; // Prix de base pour le plateau repas par défaut
            produits = new List<string> { "Entrée", "Plat", "Dessert", "Pain" };

            foreach (var supplement in supplements)
            {
                if (_supplements.TryGetValue(supplement, out decimal prix))
                {
                    total += prix;
                    produits.Add(supplement);
                }
            }

            return total;
        }
    }
}
