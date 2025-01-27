using MediatR;

namespace CantineKata.Application.Commands
{
    public class CrediterCompteCommand : IRequest<bool>
    {
        public int ClientId { get; set; }
        public decimal Montant { get; set; }
    }
}
