using CantineKata.Domain.Entities;
using CantineKata.Domain.Enums;
using CantineKata.Infrastructure.Interfaces;

namespace CantineKata.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly List<Client> _clients;

        public ClientRepository()
        {
            _clients = new List<Client>
            {
                new Client { Id = 1, Nom = "Mehdi1", TypeClient = TypeClient.Interne, Solde = 20m },
                new Client { Id = 2, Nom = "Mehdi2", TypeClient = TypeClient.Prestataire, Solde = 15m },
                new Client { Id = 3, Nom = "Mehdi3", TypeClient = TypeClient.Vip, Solde = 50m },
                new Client { Id = 4, Nom = "Mehdi4", TypeClient = TypeClient.Stagiaire, Solde = 10m },
                new Client { Id = 5, Nom = "Mehdi5", TypeClient = TypeClient.Visiteur, Solde = 5m }
            };
        }

        public async Task<Client> GetByIdAsync(int clientId) {

            return await Task.Run(() => _clients.FirstOrDefault(c => c.Id == clientId));
        }

        public async Task UpdateAsync(Client client)
        {
            await Task.Run(() =>
            {
                var existingClient = _clients.FirstOrDefault(c => c.Id == client.Id);
                if (existingClient != null)
                {
                    existingClient.Solde = client.Solde;
                }
            });
        }
    }
}
