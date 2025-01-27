using CantineKata.Application.Commands;
using CantineKata.Infrastructure.Interfaces;
using MediatR;

namespace CantineKata.Application.Handlers
{
    public class CrediterCompteHandler : IRequestHandler<CrediterCompteCommand, bool>
    {
        private readonly IClientRepository _clientRepository;

        public CrediterCompteHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<bool> Handle(CrediterCompteCommand request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId);

            if (client == null)
                return false;

            client.Solde += request.Montant;
            await _clientRepository.UpdateAsync(client);

            return true;
        }
    }
}
