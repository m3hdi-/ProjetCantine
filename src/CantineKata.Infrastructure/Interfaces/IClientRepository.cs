using CantineKata.Domain.Entities;

namespace CantineKata.Infrastructure.Interfaces;

public interface IClientRepository
{
    Task<Client> GetByIdAsync(int clientId);
    Task UpdateAsync(Client client);

}
