using CantineKata.Domain.Entities;

namespace CantineKata.Application.Interfaces
{
    public interface ITicketService
    {
        Task<string> GenererTicketAsync(Client client, List<string> produits, decimal montantTotal, decimal priseEnCharge, decimal montantPaye, DateTime date);
    }
}
