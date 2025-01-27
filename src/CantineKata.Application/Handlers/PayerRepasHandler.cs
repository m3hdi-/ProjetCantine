using CantineKata.Application.Commands;
using CantineKata.Application.Interfaces;
using CantineKata.Domain.Entities;
using CantineKata.Domain.Enums;
using CantineKata.Infrastructure.Interfaces;
using MediatR;

namespace CantineKata.Application.Handlers
{
    public class PayerRepasHandler : IRequestHandler<PayerRepasCommand, string>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITarificationService _tarificationService;
        private readonly ITicketService _ticketService;


        public PayerRepasHandler(IClientRepository clientRepository, ITarificationService tarificationService, ITicketService ticketService)
        {
            _clientRepository = clientRepository;
            _tarificationService = tarificationService;
            _ticketService = ticketService;
        }

        public async Task<string> Handle(PayerRepasCommand request, CancellationToken cancellationToken)
        {
            Client client = await _clientRepository.GetByIdAsync(request.ClientId);

            if (client == null)
                throw new Exception("Client introuvable.");

            decimal total = 10m;
            List<string> produits = new List<string>();

            total = _tarificationService.CalculTotalSupplement(request.Supplements,out produits);

            decimal priseEnCharge = _tarificationService.CalculerPriseEnchange(client, total);
            decimal montantAPayer = total - priseEnCharge;

            if (montantAPayer < 0) montantAPayer = 0;

            if (montantAPayer > client.Solde && client.TypeClient != TypeClient.Interne && client.TypeClient != TypeClient.Vip)
            {
                throw new Exception("Solde insuffisant pour effectuer le paiement.");
            }

            client.Solde -= montantAPayer;
            await _clientRepository.UpdateAsync(client);

            return await _ticketService.GenererTicketAsync(client, produits, total, priseEnCharge, montantAPayer, DateTime.Now);
        }
    }
}
