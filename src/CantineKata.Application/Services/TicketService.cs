using CantineKata.Application.Interfaces;
using CantineKata.Domain.Entities;
using System.Text;

namespace CantineKata.Application.Services
{
    public class TicketService : ITicketService
    {
        public async Task<string> GenererTicketAsync(Client client, List<string> produits, decimal montantTotal, decimal priseEnCharge, decimal montantPaye, DateTime date)
        {
            var ticketContent = new StringBuilder();

            ticketContent.AppendLine($"Ticket du Client {client.Nom}");
            ticketContent.AppendLine($"Date : {date}");
            ticketContent.AppendLine("Produits :");

            foreach (var produit in produits)
            {
                ticketContent.AppendLine($"- {produit}");
            }

            ticketContent.AppendLine($"\nMontant Total : {montantTotal}€");
            ticketContent.AppendLine($"Prise en Charge : {priseEnCharge}€");
            ticketContent.AppendLine($"Montant Payé : {montantPaye}€");

            return ticketContent.ToString();
        }
    }
}
